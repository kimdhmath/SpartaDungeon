
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
        string levelStr = string.Format("Lv. {0:D2}", level);//레벨을 2자리로 표시
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

            if(int.TryParse(Console.ReadLine(), out select))
            {
                if (select == 0)//0을 입력하면 나가기
                {
                    Console.Clear();
                    return;
                }
                else//그 외의 입력을 받으면 잘못된 입력임을 알림
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }
}

public class Inventory //인벤토리 클래스
{
    private List<Item> items = new List<Item>();
    private Character character;
    private int select;
    private int count = 0;
    private bool isEquipManage = false;

    public Inventory(Character character)
    {
        this.character = character;
    }

    public void InventoryMenu()
    {
        while (true)
        {
            ShowInventory();

            if (int.TryParse(Console.ReadLine(), out select))
            {
                if (select == 0)
                {
                    Console.Clear();
                    return;
                }
                else if (select == 1)
                {
                    isEquipManage = true;
                    EquipManage();
                    Console.Clear();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }

   
    public void EquipManage()
    {
        while(true)
        {
            ShowInventory();

            if (int.TryParse(Console.ReadLine(), out select))
            {
                if (select == 0)
                {
                    isEquipManage = false;
                    Console.Clear();
                    return;
                }
                else if (select > 0 && select <= items.Count )
                {
                    items[select - 1].equip = !items[select - 1].equip;
                    Console.Clear();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }


    public void ShowInventory()
    {
        count = 0;
        Console.Clear();
        Console.WriteLine("인벤토리");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");
        foreach (Item item in items)
        {
            if(isEquipManage)
            {
                Console.Write("-" + ++count + " ");
            }

            if (item.equip == true)
            {
                Console.Write($"[E]{item.name,-12}|");
            }
            else
            {
                Console.Write($"{item.name,-15}|");
            }

            if (item.type == 1)
            {
                Console.WriteLine($"공격력 +{item.attackPower,-4}|{item.description}");
            }
            else if (item.type == 2)
            {
                Console.WriteLine($"방어력 +{item.defensePower,-4}|{item.description}");
            }
            else if (item.type == 3)
            {
                Console.WriteLine($"체 력 +{item.hp,-4}|{item.description}");
            }
        }
        if(!isEquipManage)
        {
            Console.WriteLine("1.장비 관리");
        }
        Console.WriteLine("0.나가기");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해 주세요");
        Console.Write(">>");

    }

    public void addItem(Item item)
    {
        items.Add(item);
    }
}

public class Item //아이템 클래스
{
    public string name { get; set; }
    public int price { get; set; }
    public int attackPower { get; set; }
    public int defensePower { get; set; }
    public int hp { get; set; }
    public int type { get; set; }
    public bool equip { get; set; }
    public string description { get; set; }
    public Item(string name, int price, int attackPower, int defensePower, int hp, int type, string description)
    {
        this.name = name;
        this.price = price;
        this.attackPower = attackPower;
        this.defensePower = defensePower;
        this.hp = hp;
        this.type = type;
        this.equip = false;
        this.description = description;
    }
}
public class Village //마을 클래스
{
    private int select;
    public Village()
    {

    }

    public int SelectMenu() //마을에서 할 수 있는 행동을 선택
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

            if (int.TryParse(Console.ReadLine(), out select))
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
        Inventory inventory = new Inventory(character);
        int select = 0;
        bool isPlaying = true; //게임이 진행 중인지 확인

        Item sword = new Item("sword", 100, 5, 0, 0, 1, "test검입니다.");
        Item shield = new Item("shield", 100, 0, 5, 0, 2, "test방패입니다.");
        Item ammor = new Item("ammor", 100, 0, 0, 50, 3, "test갑옷입니다.");
        inventory.addItem(sword);
        inventory.addItem(shield);
        inventory.addItem(ammor);

        while (isPlaying)
        {
            select = village.SelectMenu();//마을에서 할 수 있는 행동을 선택
            switch (select)
            {
                case 1:
                    character.Status(); //캐릭터의 상태를 보여줌
                    break;
                case 2:
                    inventory.InventoryMenu();//인벤토리 메뉴를 보여줌
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