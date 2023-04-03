# TaskManager
This is a component that is used to manage the tasks related to a scene. 

## How to use
Create an empty in your scene and name is something logical like _GameManager_ and add a TaskManager to it as a component.

For a concrete example on how to use the TaskManager script, look at the scene located in [`/Assets/_Scenes/Examples/TaskManagerExample.unity`](/Assets/_Scenes/Examples/TaskManagerExample.unity).

## Parameters 
### [TaskList][tasklist_doc] **taskListObject**
The task list you want to use in the current scene. Please refer to the [TaskList documentation][tasklist_doc] to see create a task list.

### bool **resetOnStart**
Whether [the task list][tasklistobj_doc] should be reset when the scene is loaded.

## Public methods
### void **CompleteTask**(string **taskKey**)
Marks the task with the given key as done and selects a new task to be active.

**NOTE!** If a task is timed then the timer is only started when the task is selected as currently active. If a task is completed before it has been selected as currently active then the player will be awarded full points.

### void **GenerateTaskMistake**(string **mistakeText**, int **deductedPoints**)
Creates and stores a task mistake that is related to the currently active task.

_mistakeText_ contains the mistake message that can be shown to the player and _deductedPoints_ is how many points the player should be deducted for performing said mistake.

### void **GenerateGeneralMistake**(string **mistakeText**, int **deductedPoints**)
Creates and stores a general mistake that isn't related to any task.

_mistakeText_ contains the mistake message that can be shown to the player and _deductedPoints_ is how many points the player should be deducted for performing said mistake.

### [Task][task_doc] **GetCurrentTask()**
Returns the task that is currently active.

### bool **IsTaskCompleted**(string **taskKey**)
Returns whether or not the task with the given key has been completed.

### Task **GetTask**(string **taskKey**)
Returns the task with the given key.

[tasklist_doc]: /Docs/Architecture/tasklist.md
[tasklistobj_doc]: /Docs/Architecture/taskmanager.md#tasklist-tasklistobject
[task_doc]: /Docs/Architecture/Classes/task.md