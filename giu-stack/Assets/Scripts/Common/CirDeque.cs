using System;
/// <summary>
/// 循环双端队列
/// </summary>
/// <typeparam name="T">数据类型</typeparam>

class CirDeque<T>
{
    /// <summary>
    /// 默认队列大小
    /// </summary>
    private const int DEFAULT_SIZE = 30;
    /// <summary>
    /// 数据域
    /// </summary>
    private T[] data;
    /// <summary>
    /// 队列头
    /// </summary>
    private int front;
    /// <summary>
    /// 队列尾巴
    /// </summary>
    private int rear;
    /// <summary>
    /// 判断队列满
    /// </summary>
    /// <returns>返回是否满</returns>
    public bool IsEmpty() => this.rear == this.front;
    /// <summary>
    /// 判断队列满
    /// </summary>
    /// <returns></returns>
    public void Expand()
    {
        if ((this.rear + 1) % this.data.Length == this.front)
        {
            Array.Resize(ref this.data,
                this.data.Length + this.data.Length << 1);
        }
    }
    /// <summary>
    /// 构造器
    /// </summary>
    /// <param name="size">大小</param>
    public CirDeque(int size = DEFAULT_SIZE)
    {        
        this.data = new T[size];
        this.front = 0;
    }

    /// <summary>
    /// 队头进队
    /// </summary>
    /// <param name="val">数据元素</param>
    public void EnterFront(T val)
    {
        Expand();
        if (this.front == 0)
        {
            this.front = this.data.Length - 1;
            this.data[this.front] = val;
        }
        else
        {
            this.front = (this.front - 1) % this.data.Length;
            this.data[this.front] = val;
        }
    }
    /// <summary>
    /// 队尾进队
    /// </summary>
    /// <param name="val">数据元素</param>
    public void EnterRear(T val)
    {
        Expand();
        this.data[this.rear] = val;
        this.rear = (this.rear + 1) % this.data.Length;
    }
    /// <summary>
    /// 队头出队
    /// </summary>
    /// <param name="val">数据元素</param>
    public T OutFront()
    {
        if (IsEmpty())
            throw new IndexOutOfRangeException("队空");
        var val = this.data[this.front];
        this.front = (this.front + 1) % this.data.Length;
        return val;
    }
    /// <summary>
    /// 队尾出队
    /// </summary>
    /// <param name="val">数据元素</param>
    public T OutRear()
    {
        if (IsEmpty())
            throw new IndexOutOfRangeException("队空");
        if (this.rear == 0)
        {
            this.rear = this.data.Length - 1;
            return this.data[this.rear];
        }
        else
        {
            this.rear = (this.rear - 1) % this.data.Length;
            return this.data[this.rear];
        }

    }
    /// <summary>
    /// 得到队列有效元素个数
    /// </summary>
    /// <returns></returns>
    public int Count()
    {
        int count = this.rear - this.front;
        if(this.rear<this.front)
        {
            count = DEFAULT_SIZE - Math.Abs(this.rear - this.front);
        }
        return count;
    }
    /// <summary>
    /// 通过下标获取队列中存储元素值
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T GetValue(int index)
    {
        if (index >= this.data.Length)
        {
            return default(T);
        }

        int curIndex = (this.front + index) % this.data.Length;

        return this.data[curIndex];
    }
}