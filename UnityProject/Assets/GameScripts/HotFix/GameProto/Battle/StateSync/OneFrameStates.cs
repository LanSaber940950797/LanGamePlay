namespace ET
{
    public partial class OneFrameStates
    {
        public void CopyTo(OneFrameStates to)
        {
            to.States.Clear();
            foreach (var kv in this.States)
            {
                to.States.Add(kv.Key, kv.Value);
            }
        }
    }
    
    
}