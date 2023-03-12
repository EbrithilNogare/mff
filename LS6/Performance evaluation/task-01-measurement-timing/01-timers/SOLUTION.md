Two scripts were made to analyze and measure time measurement methods for JS:
- `Date.now()`
- `performance.now()`

Because some protections `performance.now()` was also executed in isolated mode to get better precision mode.

I chose 3 most common browsers (Chrome, firefox and Edge),
I also wanted Internet explorer but it was blocked by Windows as "Edge is a what everybody wants".

First script measures minimal step in time by calling timer repeatedly.

Second script measures time needed to execute these methods.
