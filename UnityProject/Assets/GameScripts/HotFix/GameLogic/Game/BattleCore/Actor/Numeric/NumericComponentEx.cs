namespace GameLogic.Battle
{
    public static class NumericComponentSystem2
    { 
         public static void Modify(this NumericComponent self, int nt, long value, bool isPublicEvent = true)
         {
             var curValue = self[nt];
             curValue += value;
             self.Insert(nt, curValue, isPublicEvent);
         }
    }
}