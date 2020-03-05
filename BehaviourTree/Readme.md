# Library Purpose
This library allows the creation of a re-entrant behaviour tree using a fluent API.

## Creating a Behaviour Tree: an Example
A behaviour tree is built using the fluent API against a BehaviourTreeBuilder object. The resulting tree node is then the root that is visited during the game loop interval.

This behaviour tree is re-entrant. Until the state is reset to *Ready* (will happen automatically when the root node reads something other than *Running*), it will navigate to the last known Running node, and attempt to complete it every time the Visit method is called. 

```C#
BehaviourTreeBuilder<long, object> builder = new BehaviourTreeBuilder<long, object>("Main");
var treeRoot = builder
        .Sequence("Node 1")
            .Selector("Node 2")
                .Action("Do 2", (x, y) => BehaviourReturnCode.Failure)
                .Action("Do 3", (x, y) => BehaviourReturnCode.Failure)
            .End()
            .Action("Do 1", (x, y) => BehaviourReturnCode.Success)
            .Action("Do 4", (x, y) => BehaviourReturnCode.Success)
        .End()
    .End()
    .Build();

/// ... Other Code ...

var returnStatus = treeRoot.Visit(1000, dataContext);
```

## Node States

- *Ready*: The node action is ready to run
- *Running*: The node is currently processing its action
- *Success*: The node action has suceeded
- *Failure*: The node action has failed
- *Error*: The node had an error and stopped processing

## Node Types

### Action Node
A leaf node in the behaviour tree. The elapsed time and data contect is sent to the action to help in localized processing. The action is run, with the expected return of *Running*, *Success*, or *Failure*.

```C#
.Action("Do Action Alpha", (elapsedTime, dataContext) => 
{
	// ... Do the actions here. ...
	// ... For actions happening in the background, return Running until complete ...
	return BehaviourReturnCode.Success;
});
```
### Sequence Node
Runs all child nodes in order until one returns *Failure* or they all return *Success*.

```C#
.Sequence("Sequence")
    .Action("Action", (elapsedTime, dataContext) => BehaviourReturnCode.Success)
    .Action("Action", (elapsedTime, dataContext) => BehaviourReturnCode.Success)
.End()
```

### Selector Node
Runs all child nodes in order until one returns *Success* or they all return *Failure*.

```C#
.Selector("Selector")
    .Action("Action", (elapsedTime, dataContext) => BehaviourReturnCode.Failure)
    .Action("Action", (elapsedTime, dataContext) => BehaviourReturnCode.Success)
.End()
```

### Parallel Node
Runs all child nodes at the same time, every time the Visit is called. Returns *Failure* when any fail, *Running* if any are still running, and *Success* if all have succeeded.

```C#
.Parallel("Parallel")
    .Action("Action", (elapsedTime, dataContext) => BehaviourReturnCode.Running)
    .Action("Action", (elapsedTime, dataContext) => BehaviourReturnCode.Success)
.End()
```

### Race Node
Runs all child nodes at the same time, every time the Visit is called. Returns *Success* when the first node succeeds, *Running* if any are still running, and *Failure* if all have failed.

```C#
.Race("Race")
    .Action("Action", (elapsedTime, dataContext) => BehaviourReturnCode.Running)
    .Action("Action", (elapsedTime, dataContext) => BehaviourReturnCode.Success)
.End()
```

### Random Node
Runs one of the child nodes at random, and returns its result. Will keep visiting it until it returns *Success* or *Failure*.

```C#
.Random("Random")
    .Action("Action", (elapsedTime, dataContext) => BehaviourReturnCode.Running)
    .Action("Action", (elapsedTime, dataContext) => BehaviourReturnCode.Success)
.End()
```

### Random Node
Repeats the given action a specified number of times. Returns *Success* if all attempts succeed, or *Failure* if any attempt fails.

```C#
.Repeat("Repeat", (elapsedTime, dataContext) => 
{
	BehaviourReturnCode.Running;
}, 5);
```

### Invert Node
Returns the inverse of the return value of the child action. *Success* -> *Failure*, *Failure* -> *Success*.

```C#
.Invert("Invert", (elapsedTime, dataContext) => BehaviourReturnCode.Success) // Becomes Failure
```

### Succeed Node
Returns *Success*, unless the node is still running.

```C#
.Succeed("Succeed", (elapsedTime, dataContext) => BehaviourReturnCode.Failure)
```

### Wait Node
Waits a given amount of time, then returns *Success*.

```C#
.Wait("Wait", 1000)
```

### While Node
Continues visiting the action node while the condition node returns *Success*.

If the action node returns *Failure* at any time, or is never visited at all, the while returns *Failure*. If the action has succeeded at least once then the condition node fails, the while returns *Success*.

```C#
.While("While", 
	(elapsedTime, dataContext) => 
	{
		// Condition node
		BehaviourReturnCode.Success;
	}, 
	(elapsedTime, dataContext) => 
	{
		// Action node
		BehaviourReturnCode.Failure;
	});
```

### Condition Node
Wraps around an action node, returning *Success* if the action function returns True, or *Failure* if it returns false.

```C#
.Condition("True", (elapsedTime, dataContext) => true)
```

## Current Tree State
The current state of the behaviour tree can be generated for use in debugging and diagnostics. Calling the GetState method on the behaviour tree will generate an object with the names of the nodes, the current running state of the node, and the state of the child nodes.

```C#
IBehaviourTreeState currentState = treeRoot.GetState();
```