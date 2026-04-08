namespace SkanderNET.Data
{
    internal class SkylanderExperience
    {
        internal static uint GetLevel(uint experience)
        {
            if (experience < 1000)
                return 1;
            if (experience < 2200)
                return 2;
            if (experience < 3800)
                return 3;
            if (experience < 6000)
                return 4;
            if (experience < 9000)
                return 5;
            if (experience < 13000)
                return 6;
            if (experience < 18200)
                return 7;
            if (experience < 24800)
                return 8;
            if (experience < 33000)
                return 9;
            if (experience < 42700)
                return 10;
            if (experience < 53900)
                return 11;
            if (experience < 66600)
                return 12;
            if (experience < 80800)
                return 13;
            if (experience < 96500)
                return 14;
            if (experience < 113700)
                return 15;
            if (experience < 132400)
                return 16;
            if (experience < 152600)
                return 17;
            if (experience < 174300)
                return 18;
            if (experience < 197500)
                return 19;
            return 20;
        }

        internal static uint GetExperience(uint level)
        {
            if (level == 1)
                return 0;
            if (level == 2)
                return 1000;
            if (level == 3)
                return 2200;
            if (level == 4)
                return 3800;
            if (level == 5)
                return 6000;
            if (level == 6)
                return 9000;
            if (level == 7)
                return 13000;
            if (level == 8)
                return 18200;
            if (level == 9)
                return 24800;
            if (level == 10)
                return 33000;
            if (level == 11)
                return 42700;
            if (level == 12)
                return 53900;
            if (level == 13)
                return 66600;
            if (level == 14)
                return 80800;
            if (level == 15)
                return 96500;
            if (level == 16)
                return 113700;
            if (level == 17)
                return 132400;
            if (level == 18)
                return 152600;
            if (level == 19)
                return 174300;
            if (level >= 20)
                return 197500;
            return 1;
        }
         
    }
}