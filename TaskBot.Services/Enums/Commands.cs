using System.ComponentModel;

namespace TaskBot.Services.Enums;

public enum Commands: byte
{
    [Description("/start")]
    Start,
    
    [Description("/gettasks")]
    GetTasks,
    
    [Description("/createtask")]
    CreateTask,
    
    [Description("/closetask")]
    CloseTask
}