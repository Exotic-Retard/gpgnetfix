# TODO list #
## Polling loops ##
GPGNet has a lot of code like this:
```
                        while (this.AsyncInProgress)
                        {
                            Thread.Sleep(100);
                        }
```
Where it should have been
```
this.AsyncEvent.WaitOne();
```
and on triggering:
```
this.AsyncEvent.Set();
```
with initialization:
```
this.AsyncEvent=new EventWaitHandle(false,EventResetMode.ManualReset);
```
### Why are polling loops bad ? ###
Because you'll always wait a multiple of some specific time, and the OS will have to re-schedule the thread for every check. If you use events instead, the OS will wake up the thread only when needed, and it'll be instant.
## Sleep in general ##
The entire code base is littered with Sleep calls.
These should be removed unless they are absolutely needed.
If a Sleep was put in to wait for some kind of process to end, it should be replaced by something that actually waits for the process to end, instead of a predefined number of milliseconds.