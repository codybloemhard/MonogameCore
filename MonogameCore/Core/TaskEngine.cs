using System;
using System.Collections.Generic;
using System.Threading.Tasks;
/*
namespace Core
{
    public interface _taskdata { }

    public class TaskResult<T> : _taskdata
    {
        public T data;
    }

    public class TaskEngine
    {
        public readonly uint PoolSize;
        private Task[] tasks;
        private Action<_taskdata>[] callback;

        public TaskEngine(uint poolSize)
        {
            PoolSize = poolSize;
            tasks = new Task[PoolSize];
            callback = new Action<_taskdata>[PoolSize];
            for (int i = 0; i < PoolSize; i++)
            {
                tasks[i] = null;
                callback[i] = null;
            }
        }

        public void Update()
        {
            for(int i = 0; i < PoolSize; i++)
            {
                if(tasks[i] != null)
                {
                    if (tasks[i].IsCompleted)
                    {
                        
                        tasks[i] = null;
                    }
                }
            }
        }

        public bool Add<T>(Func<T> task, Action<_taskdata> callback)
        {
            int spot = FindSpot();
            if (spot == -1) return false;
            tasks[spot] = new Task<T>(task);
            tasks[spot].Start();
            this.callback[spot] = callback;
            return true;
        }

        private int FindSpot()
        {
            for (int i = 0; i < PoolSize; i++)
                if (tasks[i] == null) return i;
            return -1;
        }
    }
}*/