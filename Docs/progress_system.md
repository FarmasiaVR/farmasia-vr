# Working with the progress system

## This guide is for the old architecture only. A recommended way to build a new task is with the [New Architecture](/tree/dev/Docs/Architecture/architecture.md)

All progress system code is in the [ProgressSystem](/tree/dev/Assets/Scripts/ProgressSystem) directory

## Adding a new Task

It is a good idea to read the current task scripts. The clearest examples of the old architechture are probably some of the MembraneFiltration tasks. 

Remember, you dont have to copy this workflow precisely, and if you feel like it, you can make changes to the whole system to make it less stupid. 
We did that multiple times, but its still somewhat stupid in places.

### Steps

1. Create a new C# file with a class that extends `Task`. For consistency you should put it in `ProgressSystem/Tasks/<scene directory>`.

2. Define some conditions in an enum for completing the task. Any number is fine.

3. Your task's constructor should look something like this
```csharp
public YourTask() : base(TaskType.YourTaskType, false) {
// all conditions have to be true to allow the task to be completed. Maybe refactor this to be always true?
  SetCheckAll(true); 
  
  // Add the conditions you defined in a Conditions enum. Again, up to you to refactor, doesnt look to pretty like this
  AddConditions((int[])Enum.GetValues(typeof(Conditions))); 
}
```
4. Add a new enum value in `ProgressSystem/TaskType.cs` for your task, and use that in the constructor in place of `YourTaskType`.

5. Adding the task to a scene: go modify `ProgressManager.cs`. Currently you have to add the TaskType to the appropriate `GenerateScenario`-method, 
for example like [here](https://github.com/MikkoHimanka/farmasia-vr/blob/f4c097185c38ee000cce3258bc4e7f24b3f4e1d2/Assets/Scripts/ProgressSystem/ProgressManager.cs#L131).
Order matters! 

6. Add it to `TaskFactory.cs`, like the other ones. Order does not matter.

7. Add a new task config for it in `TaskConfig.cs`. Just copy and modify one of the existing ones.

8. Next, you want the task to be able to listen to game events in order to check whether the correct stuff happens!
You do this by subscribing a method to an event type. See [Creating and listening to events](/tree/Docs/events.md) for details.
```csharp
// example of subscribing a method to an event type

// All the event subscriptions of a Task should happen inside the Subscribe method. See its docstring for more info.
public override void Subscribe() {
    base.SubscribeEvent(DoStuffWhenThatEventHappens, EventType.YourEventType);
}

// This is the signature that an event handler method should have!
private void DoStuffWhenThatEventHappens(CallbackData data) {

    // do stuff here, like checking conditions
}
```

9. When you want to mark some condition as completed in your event handler, use the `EnableCondition` method.

10. After enabling some condition that might lead to completion, use the `CompleteTask` method. It will finish the task if all your conditions are checked. 
Otherwise it does nothing.

## Mistakes

Mistakes can be created anywhere in code, but its a good idea to try to only create them in tasks' event handlers (or methods called by them). 
```csharp
// Inside a Task, you can use this to create a mistake related to this task
CreateTaskMistake("You made a big mistake!", 10);

// Outside a task, you can use the Task-class's static methods
Task.CreateTaskMistake(TaskType.SomeTaskType, "Another huge mistake", 10); // creates a mistake related to TaskType.SomeTaskType

Task.CreateGeneralMistake("Very big mistake that's not related to any tasks specifically", 15);
```
