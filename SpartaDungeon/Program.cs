
using System.Runtime.CompilerServices;

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
    private List<Item> items = new List<Item>();//아이템 리스트
    public Character character;//캐릭터
    private int select;//선택한 번호
    private int index = 0;//아이템 번호
    private bool isEquipManage = false;//장착 관리 모드 확인
    DisPlayManager disPlayManager = new DisPlayManager();

    public Inventory(Character character)
    {
        this.character = character;
    }


    public void InventoryMenu()//인벤토리 메뉴
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine("인벤토리");
            Console.WriteLine();
            disPlayManager.ShowItmeList(items, true, false, isEquipManage);
            if (int.TryParse(Console.ReadLine(), out select))
            {
                if (isEquipManage)//장착 관리 모드일 때
                {
                    if (select == 0)//0을 입력하면 나가기
                    {
                        isEquipManage = false;
                        Console.Clear();
                    }
                    else if (select > 0 && select <= items.Count)//아이템 번호를 입력하면 장착/해제
                    {
                        items[select - 1].isEquip = !items[select - 1].isEquip;
                        Console.Clear();
                    }
                    else//그 외의 입력을 받으면 잘못된 입력임을 알림
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                else//장착 관리 모드가 아닐 때
                {
                    if (select == 0)//0을 입력하면 나가기
                    {
                        Console.Clear();
                        return;
                    }
                    else if (select == 1)//1을 입력하면 장착 관리 모드로 전환
                    {
                        isEquipManage = true;
                        Console.Clear();
                    }
                    else//그 외의 입력을 받으면 잘못된 입력임을 알림
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }
            else//숫자가 아닌 입력을 받으면 잘못된 입력임을 알림
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
        }

    }

    public void addItem(Item item)
    {
        items.Add(item);
    }
}


public class Shop
{
    List<Item> items = new List<Item>();
    private int select;
    private bool isList = false;
    DisPlayManager disPlayManager = new DisPlayManager();
    private Inventory inventory;

    public Shop(Inventory inventory)
    {
        this.inventory = inventory;
        items.Add(new Item("수련자의 갑옷", 1000, 0, 5, 0, 2, "수련에 도움을 주는 갑옷입니다."));
        items.Add(new Item("무쇠갑옷", 2000, 0, 9, 0, 2, "무쇠로 만들어져 튼튼한 갑옷입니다."));
        items.Add(new Item("스파르타 갑옷", 3500, 0, 15, 0, 2, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다."));
        items.Add(new Item("낡은 검", 600, 2, 0, 0, 1, "쉽게 볼 수 있는 낡은 검 입니다."));
        items.Add(new Item("청동 도끼", 1700, 5, 0, 0, 1, "어디선가 사용됐던거 같은 도끼입니다."));
        items.Add(new Item("스파르타의 창", 2700, 7, 0, 0, 1, "스파르타의 전사들이 사용했다는 전설의 창입니다."));
    }


    public void ShopMenu()//상점 메뉴
    {

        Console.Clear();
        while (true)
        {
            Console.WriteLine("상점");
            Console.WriteLine();
            Console.WriteLine("[보유골드]");
            Console.WriteLine(inventory.character.gold + " G");
            Console.WriteLine();
            disPlayManager.ShowItmeList(items, false, true, isList);
            if (int.TryParse(Console.ReadLine(), out select))
            {
                if (isList)//상점 구매 목록일 때
                {
                    if (select == 0)//0을 입력하면 나가기
                    {
                        isList = false;
                        Console.Clear();
                    }
                    else if (select > 0 && select <= items.Count)//아이템 번호를 입력하면 구매
                    {
                        if (!items[select - 1].isOwn)//이미 소유한 아이템이 아닐 때
                        {
                            if(inventory.character.gold >= items[select - 1].price)//골드가 충분할 때
                            {
                                inventory.character.gold -= items[select - 1].price;
                                items[select - 1].isOwn = true;
                                inventory.addItem(items[select - 1]);
                                Console.Clear();
                            }
                            else//골드가 부족할 때
                            {
                                Console.Clear();
                                Console.WriteLine("골드가 부족합니다.");
                            }
                        }
                        else
                        {
                            Console.Clear();
                        }
                    }
                    else//그 외의 입력을 받으면 잘못된 입력임을 알림
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                else//상점 메뉴일 때
                {
                    if (select == 0)//0을 입력하면 나가기
                    {
                        Console.Clear();
                        return;
                    }
                    else if (select == 1)//1을 입력하면 상점 구매 목록으로 전환
                    {
                        isList = true;
                        Console.Clear();
                    }
                    else//그 외의 입력을 받으면 잘못된 입력임을 알림
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }
            else//숫자가 아닌 입력을 받으면 잘못된 입력임을 알림
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }
}

public class Item //아이템 클래스
{
    public string name { get; set; }
    public string description { get; set; }
    public int price { get; set; }
    public int attackPower { get; set; }
    public int defensePower { get; set; }
    public int hp { get; set; }
    public int type { get; set; }
    public bool isEquip { get; set; }
  
    public bool isOwn { get; set; }

    public Item(string name, int price, int attackPower, int defensePower, int hp, int type, string description)
    {
    this.name = name;
    this.price = price;
    this.attackPower = attackPower;
    this.defensePower = defensePower;
    this.hp = hp;
    this.type = type;
    this.isEquip = false;
    this.description = description;
    this.isOwn = false;
    }
}

public class DisPlayManager
{

    public void ShowItmeList(List<Item> items, bool isInventory, bool isShop, bool isList)
    {
        int index = 0;
        Console.WriteLine("[아이템 목록]");
        foreach (Item item in items)
        {
            if (isList)//아이템 목록 표시
            {
                Console.Write("-" + ++index + " ");//아이템 번호 표시
            }

            if (item.isEquip == true && isInventory)//장착 중인 아이템 표시
            {
                Console.Write($"[E]{item.name,-12}|");
            }
            else//장착 중이 아닌 아이템 표시
            {
                Console.Write($"{item.name,-15}|");
            }

            if (item.type == 1)//아이템 타입에 따라 아이템 설명 표시
            {
                Console.Write($"공격력 +{item.attackPower,-4}|{item.description}");
            }
            else if (item.type == 2)
            {
                Console.Write($"방어력 +{item.defensePower,-4}|{item.description}");
            }
            else if (item.type == 3)
            {
                Console.Write($"체  력 +{item.hp,-4}|{item.description}");
            }

            if (isShop)//상점 구매 목록일 때
            {
                if(item.isOwn == true)//이미 소유한 아이템 표시
                {
                    Console.WriteLine($"| 구매완료");
                }
                else//아직 소유하지 않은 아이템 표시
                {
                    Console.WriteLine($"| {item.price} G");
                }
            }
            else
            {
                Console.WriteLine();
            }
        }
        Console.WriteLine();
        if (isInventory && !isList)//장착 관리 모드일 때
        {
            Console.WriteLine("1.장착 관리");
        }
        else if (isShop && !isList)//상점일 때
        {
            Console.WriteLine("1.구매");
        }
        Console.WriteLine("0.나가기");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해 주세요");
        Console.Write(">>");
        
    }
}

public class Village //마을 클래스
{
    private int select;
    DisPlayManager disPlayManager = new DisPlayManager();
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
        Shop shop = new Shop(inventory);
        int select = 0;
        bool isPlaying = true; //게임이 진행 중인지 확인

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
                    shop.ShopMenu();//상점 메뉴를 보여줌
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }
    }
}
