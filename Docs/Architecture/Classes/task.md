# Task
This document presents the parameters and functions of a Task object. New Task objects should only be generated in a Task List file.

**Note!** This class belongs to the `FarmasiaVR.New` namespace. If you want to make references to an object of this class, add
```C#
using FarmasiaVR.New;
```
to your script.

## Properties
### string **key** 
The key of the task. Use this to refer tasks during runtime. Read-only outside of the object.

### string **taskText**
What the task is / the title of the task. Use this to describe the task to the player. Read-only outside of the object.

### string **hint**
The hint related to the task. Use this to give the player some help when they are stuck or need assistance. Read-only outside of the object.

### int **points**
How many points should the player receive when the task is completed. Read-only outside of the object.

### bool **completed**
Whether the task is completed or not. Read-only outside of the object.

### bool **timed**
Whether the task is timed. If the task is timed, the points awarded are dependant on the time it takes to finish the task. Right now, the points multiplier is calculated linearly. If the player has 3/4 of the time remaining when he finishes the task, he will receive 3/4 of the points. Read-only outside of the object.

### float **timeToCompleteTask**
If [_timed_][timed_doc] is set to _true_, then how much time the player has to complete the task. If the time runs out, then the player is awarded 0 points no matter how much time has passed. Read-only outside of the object. Read-only outside of the object.

### bool **failWhenOutOfTime**
Whether the player should be penalised for not completing the task on time. Read-only outside of the object.

### float **timeTaskStarted**
The time when the task's timer was started.

### float **timeTakenToCompleteTask**
The time between starting the task's countdown and marking the task as finished. Read-only outside of the object.

### int **awardedPoints**
How many points are given to the player for completing the task. If _[timed][timed_doc]_ is set to _false_, then `awardedPoints = points`. Otherwise, the awarded points are calculated based on the time the player took to complete the task.

### List<[Mistake][mistake_doc]> **mistakeList** 
The mistakes the player has made relating to this task.

## Methods
### void **Reset**()
Resets the current task by setting _[completed][completed_doc]_ to false, resetting _[awardedPoints][awarded_doc]_ and resetting _[mistakeList][mistakelist_doc]_.

## **AddMistake**([Mistake][mistake_doc] **mistake**)
Takes an object of [Mistake][mistake_doc] and adds it to the list of made mistakes ([mistakeList][mistakelist_doc])

[completed_doc]: /Docs/Architecture/Classes/task.md#bool-completed
[timed_doc]: /Docs/Architecture/Classes/task.md#bool-timed
[awarded_doc]: /Docs/Architecture/Classes/task.md#int-awardedpoints
[mistakelist_doc]: /Docs/Architecture/Classes/task.md#listmistake-mistakelist
[mistake_doc]: /Docs/Architecture/Classes/mistake.md