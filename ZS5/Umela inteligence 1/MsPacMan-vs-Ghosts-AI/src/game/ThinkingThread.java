package game;

class ThinkingThread extends Thread 
{
	private boolean thinking = false;
    private IThinkingMethod method;
    private boolean alive;
    
    public ThinkingThread(String name, IThinkingMethod method) 
    {
    	super(name);
        this.method = method;
        alive=true;
        start();
    }

    public synchronized void kill() 
    {
        alive=false;
        notify();
    }
    
    public synchronized void startThinking()
    {
        if (!thinking) {
            thinking = true;
            notify();
        }
    }

    public synchronized boolean waitForResult(long until) {
        while (thinking && System.currentTimeMillis() < until) {
            try {
                wait(until - System.currentTimeMillis());
            } catch (InterruptedException e) {
                throw new RuntimeException(e);
            }
        }
        return !thinking;
    }

    public void run() 
    {
        while (true) {
            // Wait until we should think.
            synchronized (this) {
                while (alive && !thinking)
                    try {
                        wait();
                    } catch (InterruptedException e) {
                        throw new RuntimeException(e);
                    }

                if (!alive)
                    return;
            }

            method.think();

            // Report that we are done.
            synchronized (this) {
                thinking = false;
                notify();
            }
        }
    }
}
