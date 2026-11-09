namespace ASM1
{
    public delegate double SalaryCaculation(double salary, string position);
    public static class SalaryCalculator
    {
        public static double CalculateYearlySalary(double salary, string position)
        {
            double yearly = salary * 12;
            if(position.Equals("Manager", StringComparison.OrdinalIgnoreCase))
            {
                yearly *= 1.5;
            }
            else if (position.Equals("Developer", StringComparison.OrdinalIgnoreCase))
            {
                yearly *= 1.3;
            }
            else if (position.Equals("Tester", StringComparison.OrdinalIgnoreCase))
            {
                yearly *= 1.2;
            }
            return yearly;  
        }
    }
}
