
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;

public enum ItemType //아이템 타입
{
    Weapon,
    Armor
}

public class Character //캐릭터 클래스
{
    public int level { get; set; }
    public string name { get; set; }
    public string cClass { get; set; }
    public int attackPower { get; set; }
    public int defensePower { get; set; }
    public int maxHp { get; set; }
    public int hp { get; set; }
    public int gold { get; set; }
    public Dictionary<ItemType, Item> equipItem = new Dictionary<ItemType, Item>();

    public Character()
    {
        level = 1;
        name = "Kim";
        cClass = "전사";
        attackPower = 10;
        defensePower = 5;
        maxHp = 100;
        hp = maxHp;
        gold = 11500;
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
    public void HelthDown()
    {
        Console.WriteLine("체력이 10 감소합니다.");
        hp -= 10;
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
                        if(items[select - 1].isEquip == true)//장착 중일 때
                        {
                            UnEquipItem(items[select - 1]);
                            Console.Clear();
                        }
                        else//장착 중이 아닐 때
                        {
                            EquipItem(items[select - 1]);
                            Console.Clear();
                        }
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

    public void AddItem(Item item)//아이템 추가
    {
        items.Add(item);
    }

    public void RemoveItem(Item item)//아이템 제거
    {
        if(item.isEquip == true)//장착 중일 때
        {
            UnEquipItem(item);
        }
        items.Remove(item);
        for(int i = 0; i < items.Count; i++)//아이템 번호 재설정
        {
            Console.WriteLine(items[i].name + "  " + items[i].isEquip);
        }
    }

    private void EquipItem(Item item)//아이템 장착
    {
        item.isEquip = true;
        if (character.equipItem.ContainsKey(item.type))//이미 장착 중인 아이템이 있을 때
        {
            UnEquipItem(character.equipItem[item.type]);
            character.equipItem[item.type] = item;
        }
        else//장착 중인 아이템이 없을 때
        {
            character.equipItem.Add(item.type, item);
        }
        character.attackPower += item.attackPower;
        character.defensePower += item.defensePower;
    }

    private void UnEquipItem(Item item)//아이템 해제
    {
        item.isEquip = false;
        character.equipItem.Remove(item.type);
        character.attackPower -= item.attackPower;
        character.defensePower -= item.defensePower;
    }
    public List<Item> ItemList//아이템 리스트 반환
    {
        get { return items; }
    }
}


public class Shop
{
    List<Item> items = new List<Item>();
    private int select;
    private bool isBuyList = false;
    private bool isSaleList = false;
    DisPlayManager disPlayManager = new DisPlayManager();
    private Inventory inventory;

    public Shop(Inventory inventory)
    {
        this.inventory = inventory;
        items.Add(new Item("수련자의 갑옷", 1000, 0, 5, ItemType.Armor, "수련에 도움을 주는 갑옷입니다."));
        items.Add(new Item("무쇠갑옷", 2000, 0, 9, ItemType.Armor, "무쇠로 만들어져 튼튼한 갑옷입니다."));
        items.Add(new Item("스파르타 갑옷", 3500, 0, 15, ItemType.Armor, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다."));
        items.Add(new Item("낡은 검", 600, 2, 0, ItemType.Weapon, "쉽게 볼 수 있는 낡은 검 입니다."));
        items.Add(new Item("청동 도끼", 1700, 5, 0, ItemType.Weapon, "어디선가 사용됐던거 같은 도끼입니다."));
        items.Add(new Item("스파르타의 창", 2700, 7, 0, ItemType.Weapon, "스파르타의 전사들이 사용했다는 전설의 창입니다."));
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
            if(isBuyList)//상점 구매 목록
            {
                Console.WriteLine("[구매 목록]");
                Console.WriteLine();
                disPlayManager.ShowItmeList(items, false, true, true);
            }
            else if(isSaleList)//상점 판매 목록
            {
                Console.WriteLine("[판매 목록]");
                Console.WriteLine();
                disPlayManager.ShowItmeList(inventory.ItemList, false, true, true);
            }
            else//상점 목록
            {
                Console.WriteLine("[상점 목록]");
                Console.WriteLine();
                disPlayManager.ShowItmeList(items, false, true, false);
            }
            if (int.TryParse(Console.ReadLine(), out select))
            {
                if (isBuyList)//상점 구매 목록일 때
                {
                    if (select == 0)//0을 입력하면 나가기
                    {
                        isBuyList = false;
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
                                inventory.AddItem(items[select - 1]);
                                Console.Clear();
                            }
                            else//골드가 부족할 때
                            {
                                Console.Clear();
                                Console.WriteLine("골드가 부족합니다.");
                            }
                        }
                        else//이미 소유한 아이템일 때
                        {
                            Console.Clear();
                            Console.WriteLine("이미 소유한 아이템입니다.");
                        }
                    }
                    else//그 외의 입력을 받으면 잘못된 입력임을 알림
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
                else if (isSaleList)
                {
                    Console.Clear();
                    if (select == 0)//0을 입력하면 나가기
                    {
                        isSaleList = false;
                        Console.Clear();
                    }
                    else if (select > 0 && select <= inventory.ItemList.Count)//아이템 번호를 입력하면 판매
                    {
                        if (inventory.ItemList[select - 1].isOwn)//아이템을 소유하고 있을 때
                        {
                            inventory.character.gold += (inventory.ItemList[select - 1].price * 85) / 100;
                            inventory.ItemList[select - 1].isOwn = false;

                            Console.Clear();
                            inventory.RemoveItem(inventory.ItemList[select - 1]);
                        }
                        else//아이템을 소유하고 있지 않을 때
                        {
                            Console.Clear();
                            Console.WriteLine("소유하고 있지 않은 아이템입니다.");
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
                        isBuyList = true;
                        Console.Clear();
                    }
                    else if(select == 2)
                    {
                        isSaleList = true;
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
    public ItemType type { get; set; }
    public bool isEquip { get; set; }
  
    public bool isOwn { get; set; }

    public Item(string name, int price, int attackPower, int defensePower, ItemType type, string description)
    {
    this.name = name;
    this.price = price;
    this.attackPower = attackPower;
    this.defensePower = defensePower;
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

            if (item.type == ItemType.Weapon)//아이템 타입에 따라 아이템 설명 표시
            {
                Console.Write($"공격력 +{item.attackPower,-4}|{item.description}");
            }
            else if (item.type == ItemType.Armor)
            {
                Console.Write($"방어력 +{item.defensePower,-4}|{item.description}");
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
            Console.WriteLine("1.아이템 구매");
            Console.WriteLine("2.아이템 판매");
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
    private Character character;
    public Village(Character character)
    {
        this.character = character;
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
            Console.WriteLine("4.던전입장");
            Console.WriteLine("5.휴식");
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

    public void Rest()
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine("500 G 를 내면 체력을 회복할 수 있습니다.(보유 골드 : {0} G)", character.gold);
            Console.WriteLine();
            Console.WriteLine("1.휴식하기");
            Console.WriteLine("0.나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해 주세요");
            Console.Write(">>");
            if (int.TryParse(Console.ReadLine(), out select))
            {
                Console.Clear();
                if (select == 1)
                {
                    Console.Clear();
                    if (character.gold >= 500)
                    {
                        if (character.hp == character.maxHp)
                        {
                            Console.Clear();
                            Console.WriteLine("이미 체력이 가득 찼습니다.");
                        }
                        else if (character.hp < character.maxHp)
                        {
                            character.hp = character.maxHp;
                            character.gold -= 500;
                            Console.Clear();
                            Console.WriteLine("휴식을 완료했습니다.");
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("골드가 부족합니다.");
                    }
                }
                else if (select == 0)
                {
                    break;
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

    class Program
{
    static void Main(string[] args)
    {
        Character character = new Character();
        Village village = new Village(character);
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
                case 4:
                    character.HelthDown();
                    Console.WriteLine("던전입장");
                    break;
                case 5:
                    village.Rest();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }
    }
}
