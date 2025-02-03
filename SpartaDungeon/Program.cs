
public class Character //캐릭터 클래스
{
    public int level { get; set; }
    public string name { get; set; }
    public string cClass { get; set; }
    public int attackPower { get; set; }
    public int defensePower { get; set; }
    public int hp { get; set; }
    public int gold { get; set; }

    public Character()
    {
        level = 1;
        name = "Kim";
        cClass = "전사";
        attackPower = 10;
        defensePower = 5;
        hp = 100;
        gold = 1500;
    }

    public void Status() //캐릭터의 상태를 보여줌
    {
        int select;
        string levelStr = string.Format("Lv. {0:D2}", level);
        while (true)
        {
            Console.WriteLine("Lv. " + levelStr);
            Console.WriteLine(name + " ( " + cClass + " )");
            Console.WriteLine("공격력 : " + attackPower);
            Console.WriteLine("방어력 : " + defensePower);
            Console.WriteLine("체 력 : " + hp);
            Console.WriteLine("Gold : " + gold + " G");
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해 주세요");
            Console.Write(">>");

            select = int.Parse(Console.ReadLine());

            if (select == 0)
            {
                Console.Clear();
                return;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }
}

public class Village //마을 클래스
{
    private int select;
    public Village()
    {

    }

    public int SelectMenue() //마을에서 할 수 있는 행동을 선택
    {
        while (true)
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine("1.상태보기");
            Console.WriteLine("2.인벤토리");
            Console.WriteLine("3.상점");
            Console.WriteLine("원하시는 행동을 입력해 주세요");
            Console.Write(">>");
            select = int.Parse(Console.ReadLine());
            if (select >= 1 && select < 4)
            {
                Console.Clear();
                return select;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
        }

    }
}

class Program
{
    static void Main(string[] args)
    {
        Village village = new Village();
        Character character = new Character();
        int select = 0;
        bool isPlaying = true;

        while (isPlaying)
        {
            select = village.SelectMenue();
            switch (select)
            {
                case 1:
                    character.Status();
                    break;
                case 2:
                    Console.WriteLine("인벤토리");
                    break;
                case 3:
                    Console.WriteLine("상점");
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }
    }
}