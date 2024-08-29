using System.Resources;
using User32Wrapper;

namespace App;

public partial class Form1 : Form
{
    const int LogsLimit = 100;

    private readonly NotifyIcon _notifyIcon = new();
    private readonly ResourceManager _resources = new(typeof(Form1));
    private readonly ContextMenuStrip _iconMenu = new();
    private readonly Icon _appIcon;
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
        _appIcon = (_resources.GetObject("AppIcon") as Icon)!;
        Icon = _appIcon;

        btnStop.Location = btnStart.Location;
        btnStop.Visible = false;
        BackColor = Color.FromArgb(255, 175, 163);

        _timer.Interval = (int)numInterval.Value * 1000;
        _timer.Tick += Timer_Tick;
        btnStart.Click += BtnStart_Click;
        btnStop.Click += BtnStop_Click;
        FormClosing += Form1_FormClosing;

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

        _notifyIcon.Icon = _appIcon;
        _notifyIcon.Visible = true;
        _notifyIcon.ContextMenuStrip = _iconMenu;
        _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
    }

    private void MenuExit_Click(object? sender, EventArgs e)
    {
        Application.Exit();
    }

    private void NotifyIcon_DoubleClick(object? sender, EventArgs e)
    {
        Show();
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
}

enum TimerState
{
    Running,
    Stopped,
}
