using Microsoft.Win32;
using System.Resources;
using System.Text.Json;
using User32Wrapper;

namespace App;

public partial class Form1 : Form
{
    const int LogsLimit = 100;
    
    private Settings _settings;
    private readonly NotifyIcon _notifyIcon = new();
    private readonly ResourceManager _resources = new(typeof(Form1));
    private readonly ContextMenuStrip _iconMenu = new();
    private Icon _appIcon;
    private readonly System.Windows.Forms.Timer _timer = new();
    private readonly Queue<DateTime> _timestamps = new();
    private TimerState _timerState = TimerState.Stopped;
    private readonly User32.INPUT _input = new()
    {
        type = User32.InputType.INPUT_MOUSE,
        U = new User32.InputUnion()
        {
            mi = new User32.MOUSEINPUT()
            {
                dwFlags = User32.MOUSEEVENTF.ABSOLUTE,
                dx = 0,
                dy = 0,
            }
        }
    };

    public Form1()
    {
        InitializeComponent();

        btnStop.Location = btnStart.Location;
        btnStop.Visible = false;
        BackColor = Color.FromArgb(255, 175, 163);

        _timer.Interval = (int)numInterval.Value * 1000;
        _timer.Tick += Timer_Tick;
        btnStart.Click += BtnStart_Click;
        btnStop.Click += BtnStop_Click;
        FormClosing += Form1_FormClosing;
        Load += Form1_Load;

        var menuStart = new ToolStripMenuItem("Start");
        menuStart.Name = "Start";
        menuStart.Click += BtnStart_Click;
        _iconMenu.Items.Add(menuStart);
        var menuStop = new ToolStripMenuItem("Stop");
        menuStop.Name = "Stop";
        menuStop.Visible = false;
        menuStop.Click += BtnStop_Click;
        _iconMenu.Items.Add(menuStop);
        var menuExit = new ToolStripMenuItem("Exit");
        menuExit.Click += MenuExit_Click;
        _iconMenu.Items.Add(menuExit);
    }

    private void Form1_Load(object? sender, EventArgs e)
    {
        LoadSettings();

        _appIcon = (_resources.GetObject("AppIcon") as Icon)!;
        Icon = _appIcon;

        chkAutoStart.Checked = _settings!.AutoStart;
        chkAutoStart.CheckedChanged += ChkAutoStart_CheckedChanged;
        chkRunAtStartup.Checked = _settings!.RunAtStartup;
        chkRunAtStartup.CheckedChanged += ChkRunAtStartup_CheckedChanged;

        _notifyIcon.Icon = _appIcon;
        _notifyIcon.Visible = true;
        _notifyIcon.ContextMenuStrip = _iconMenu;
        _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

        if (_settings.AutoStart)
            BtnStart_Click(null, new EventArgs());

        if (_settings.AutoStart)
        {
            Visible = false;
            ShowInTaskbar = false;
        }
    }

    private void ChkRunAtStartup_CheckedChanged(object? sender, EventArgs e)
    {
        if (chkRunAtStartup.Checked)
            RunAtStartup();
        else
            DontRunAtStartup();
        _settings.RunAtStartup = chkRunAtStartup.Checked;
        SaveSettings();
    }

    private void ChkAutoStart_CheckedChanged(object? sender, EventArgs e)
    {
        _settings.AutoStart = chkAutoStart.Checked;
        SaveSettings();
    }

    private void MenuExit_Click(object? sender, EventArgs e)
    {
        Application.Exit();
    }

    private void NotifyIcon_DoubleClick(object? sender, EventArgs e)
    {
        Show();
        ShowInTaskbar = true;
    }

    private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            Hide();
        }
    }

    private void BtnStop_Click(object? sender, EventArgs e)
    {
        if (_timerState == TimerState.Stopped)
            return;

        _timer.Stop();
        _timerState = TimerState.Stopped;
        btnStart.Visible = true;
        btnStop.Visible = false;
        _iconMenu.Items["Start"]!.Visible = true;
        _iconMenu.Items["Stop"]!.Visible = false;
        BackColor = Color.FromArgb(255, 175, 163);
    }

    private void BtnStart_Click(object? sender, EventArgs e)
    {
        if (_timerState == TimerState.Running)
            return;

        _timer.Interval = (int)numInterval.Value * 1000;
        _timer.Start();
        _timerState = TimerState.Running;
        btnStart.Visible = false;
        btnStop.Visible = true;
        _iconMenu.Items["Start"]!.Visible = false;
        _iconMenu.Items["Stop"]!.Visible = true;
        BackColor = Color.FromArgb(186, 255, 179);
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        User32.SendInput(1, [_input], User32.INPUT.Size);

        if (_timestamps.Count >= LogsLimit)
            _timestamps.Dequeue();
        _timestamps.Enqueue(DateTime.Now);
        var logs = _timestamps.Select(x => $"Mouse event sent at {x:dd/MM/yyyy HH:mm:ss}.");
        txtLogs.Text = string.Join("\r\n", logs);
    }

    private void LoadSettings()
    {
        var path = Path.Join(Path.GetDirectoryName(Application.ExecutablePath), "settings.json");
        if (!File.Exists(path))
            File.WriteAllText(path, "{}");
        var file = File.Open(path, FileMode.Open, FileAccess.Read);
        var reader = new StreamReader(file);
        var content = reader.ReadToEnd();
        file.Close();

        _settings = JsonSerializer.Deserialize<Settings>(content)!;
    }

    private void SaveSettings()
    {
        var path = Path.Join(Path.GetDirectoryName(Application.ExecutablePath), "settings.json");
        var content = JsonSerializer.Serialize(_settings);
        File.WriteAllText(path, content);
    }

    private void RunAtStartup()
    {
        using var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)!;
        key.SetValue(Application.ProductName, $"\"{Application.ExecutablePath}\"");
        key.Close();
    }

    private void DontRunAtStartup()
    {
        using var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)!;
        key.DeleteValue(Application.ProductName!, false);
        key.Close();
    }
}

enum TimerState
{
    Running,
    Stopped,
}
