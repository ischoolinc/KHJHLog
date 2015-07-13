using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KHJHLog
{
    public static class ClassOrderExtension
    {
        public static void CalculateClassOrder(this List<ClassOrder> ClassOrders)
        {
            Dictionary<string, List<ClassOrder>> DicClassOrders = new Dictionary<string, List<ClassOrder>>();

            foreach (ClassOrder vClassOrder in ClassOrders)
            {                
                //string GradeYear = vClassOrder.ClassName.Substring(0, 1);
                string GradeYear = vClassOrder.GradeYear.ToString();

                if (!DicClassOrders.ContainsKey(GradeYear))
                    DicClassOrders.Add(GradeYear, new List<ClassOrder>());

                DicClassOrders[GradeYear].Add(vClassOrder);
            }

            foreach (string GradeYear in DicClassOrders.Keys)
                FillClassOrder(DicClassOrders[GradeYear]);
                     
        }

        public static int GetInt(this string value)
        {
            int result;

            if (int.TryParse(value, out result))
                return result;
            else
                return 0; 
        }

        private static void FillClassOrder(List<ClassOrder> ClassOrders)
        {
            ClassOrders = ClassOrders
                .OrderBy(x => x.ClassStudentCountValue)
                .ToList();

            List<string> ClassOrderNumbers = new List<string>();

            List<string> DupClassOrderNumbers = new List<string>();


            for (int i = 0; i < ClassOrders.Count; i++)
            {
                ClassOrders[i].ClassOrderNumber = "" + (ClassOrders[i].ClassStudentCountValue * 100);

                if (!ClassOrderNumbers.Contains(ClassOrders[i].ClassOrderNumber))
                    ClassOrderNumbers.Add(ClassOrders[i].ClassOrderNumber);
                else
                    DupClassOrderNumbers.Add(ClassOrders[i].ClassOrderNumber);
            }

            foreach (string Number in DupClassOrderNumbers)
            {
                List<ClassOrder> DupClassOrders = ClassOrders
                    .FindAll(x => x.ClassOrderNumber.Equals(Number));

                #region 對於無特殊生人數做處理
                List<ClassOrder> NoSpecials = DupClassOrders
                    .FindAll(x => GetInt(x.NumberReduceCount).Equals(0));

                NoSpecials = NoSpecials.OrderBy(x => GetInt(x.ClassName)).ToList();

                for(int i=0;i<NoSpecials.Count;i++)
                    NoSpecials[i].ClassOrderNumber = "" + (GetInt(NoSpecials[i].ClassOrderNumber) - (NoSpecials.Count -i ));
                #endregion

                #region 對於有特殊生人數做處理
                List<ClassOrder> Specials = DupClassOrders
                    .FindAll(x => GetInt(x.NumberReduceCount)>0);

                Specials = Specials
                    .OrderBy(x => GetInt(x.NumberReduceCount))
                    .ThenBy(x => GetInt(x.ClassName)).ToList();

                for(int i=0;i<Specials.Count;i++)
                    Specials[i].ClassOrderNumber = "" + (GetInt(Specials[i].ClassOrderNumber) + i);
                #endregion
            }

            #region 將所有值全部重新排序
            ClassOrders = ClassOrders.OrderBy(x => GetInt(x.ClassOrderNumber)).ToList();

            for (int i = 0; i < ClassOrders.Count; i++)
                ClassOrders[i].ClassOrderNumber = "" + (i + 1);
            #endregion
        }
    }
}