# TaskList
This is a scriptable object that is used to store tasks in a file. A new task list is created by right clicking in the project directory and selecting `Create -> FarmasiaVR -> Task List`

![How to create a tasklist object](https://user-images.githubusercontent.com/9552313/224308934-b0af54ba-87b3-4bc2-b566-cd3b4cca8377.png)

An example of a task list can be found in [`Assets/Task Lists/ExampleTasks`](/Assets/Task%20Lists/ExampleTasks.asset)

## Properties
### List<[Task][task_doc]> **tasks** 
This is the list of tasks attached to this object. This list can and should be modified directly in the inspector. Refer to the [Task documentation][task_doc] for information on the different parameters.

### private int **points**
The amount of points the player has collected while completing the tasks set in the [task list][tasklist_doc].

### private Dictionary<string, [Task][task_doc]> **taskDict**
A dictionary where the key is a key of a task and the value is the task that has the key set as the [key][taskkey_doc].

### private List<[Mistake][mistake_doc]> **generalMistakes**
A list of mistakes the player has made that cannot be attributed to a specific task. For example, opening the hint box.

## Public methods
### void **MarkTaskAsDone**(string **taskKey**)
Marks the task with the given key as done and adds the points awarded from the task to the points counter.

### void [Task][task_doc] **GetTask**(string **taskKey**)
Returns the task object with the given key.

### void **ResetTaskProgression**()
Resets all of the tasks in the [tasklist](tasklist_doc), resets the [general mistakes][genmistakes_doc] and resets the [points counter][points_doc].

### void **GenerateTaskMistake**(string **taskKey**, [Mistake][mistake_doc] mistake)
Generates a mistake related to a task.

taskKey is the key of the task where the mistake was made and mistake is an object of class [Mistake][mistake_doc] which has information about the mistake.

### void **GenerateGeneralMistake**([Mistake][mistake_doc] mistake)
Generates a mistake that doesn't relate to a certain task.

mistake is an object of class [Mistake][mistake_doc] which has information about the mistake.

### int **GetPoints**()
Returns the points the player has collected so far.

### List<[Mistake][mistake_doc]> **GetGeneralMistakes()**
Returns the general mistakes the player has made.

[task_doc]: /Docs/Architecture/Classes/task.md
[tasklist_doc]: /Docs/Architecture/tasklist.md#listtask-tasks
[taskkey_doc]: /Docs/Architecture/Classes/task.md#key-string
[mistake_doc]: /Docs/Architecture/Classes/mistake.md
[genmistakes_doc]: /Docs/Architecture/tasklist.md#private-listmistake-generalmistakes
[points_doc]: /Docs/Architecture/tasklist.md#private-int-points