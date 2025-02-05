
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;


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
    public float baseAttackPower { get; set; }
    public int baseDefensePower { get; set; }
    public float attackPower { get; set; }
    public int defensePower { get; set; }
    public int maxHp { get; set; }
    public int hp { get; set; }
    public int gold { get; set; }
    public Dictionary<ItemType, Item> equipItem = new Dictionary<ItemType, Item>();

    public Character()
    {

        name = "Kim";
        cClass = "전사";
        if (File.Exists("SaveCharacter.txt") == false)
        {
            level = 1;
            baseAttackPower = 10;
            baseDefensePower = 5;
            attackPower = baseAttackPower;
            defensePower = baseDefensePower;
            maxHp = 100;
            hp = maxHp;
            gold = 1500;
        }
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
            else//숫자가 아닌 입력을 받으면 잘못된 입력임을 알림
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }
    

    public void Die() //캐릭터 사망
    {
        Console.Clear();
        Console.WriteLine("사망하였습니다.");
        level = 1;
        baseAttackPower = 10;
        baseDefensePower = 5;
        attackPower = baseAttackPower;
        defensePower = baseDefensePower;
        maxHp = 100;
        hp = maxHp;
        gold = 1500;
    }

    public void LevelUp() //레벨업
    {
        if(level <6)//레벨이 5 이하일 때
        {
            level++;
            baseAttackPower += 0.5f;
            baseDefensePower += 1;
            CalculateAbility();
        }
    }

    public void EquipItem(Item item) //아이템 장착
    {
        if (equipItem.ContainsKey(item.type))
        {
            UnEquipItem(equipItem[item.type]);
            equipItem[item.type] = item;
        }

        equipItem[item.type] = item; // 새 아이템 장착
        CalculateAbility(); // 능력치 계산
    }

    public void UnEquipItem(Item item)//아이템 해제
    {
        equipItem.Remove(item.type);//아이템 해제
        CalculateAbility();//능력치 계산
    }

    public void CalculateAbility()//능력치 계산
    {
        attackPower = baseAttackPower;
        defensePower = baseDefensePower;
        foreach (Item item in equipItem.Values)//장착 중인 아이템의 능력치를 더함
        {
            attackPower += item.attackPower;
            defensePower += item.defensePower;
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

    public Inventory(Character character)
    {
        this.character = character;
    }


    public void InventoryMenu()//인벤토리 메뉴
    {
        var sortList = items.OrderBy(x => x.id);
        items = sortList.ToList();
        Console.Clear();
        while (true)
        {
            Console.WriteLine("인벤토리");
            Console.WriteLine();
            ShowItmeList(items, true, false, isEquipManage);
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
                        if (items[select - 1].isEquip == true)//장착 중일 때
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
        if (item.isEquip == true)//장착 중일 때
        {
            EquipItem(item);
        }
    }

    public void RemoveItem(Item item)//아이템 제거
    {
        if(item.isEquip == true)//장착 중일 때
        {
            UnEquipItem(item);
        }
        if(item.isOwn == true)//소유 중일 때
        {
            item.isOwn = false;
        }
        items.Remove(item);
    }

    public void RemoveAllItem() // 모든 아이템 제거
    {
        var itemsCopy = new List<Item>(items);
        foreach (Item item in itemsCopy)
        {
            RemoveItem(item);
        }
        character.Die();
    }

    private void EquipItem(Item item)//아이템 장착
    {
        item.isEquip = true;

        if (character.equipItem.ContainsKey(item.type)) // 이미 같은 타입의 아이템이 장착 중
        {
            UnEquipItem(character.equipItem[item.type]); // 기존 아이템 해제
        }
        character.EquipItem(item); // 캐릭터의 능력치 갱신
    }

    private void UnEquipItem(Item item)//아이템 해제
    {
        item.isEquip = false;
        character.UnEquipItem(item); // 캐릭터의 능력치 갱신
    }
    public List<Item> ItemList//아이템 리스트 반환
    {

        get { return items; }
    }

    public void ShowItmeList(List<Item> items, bool isInventory, bool isShop, bool isList)
    {
        int index = 0;
        var sortList = items.OrderBy(x => x.id);
        items = sortList.ToList();
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
                if (item.isOwn == true)//이미 소유한 아이템 표시
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


public class Shop
{
    List<Item> items = new List<Item>();
    List<int> index = new List<int>();
    private int select;
    private bool isBuyList = false;
    private bool isSaleList = false;
    private Inventory inventory;

    public Shop(Inventory inventory)
    {
        this.inventory = inventory;
        foreach(Item item in inventory.ItemList)//저장된 인벤토리에 있는 아이템을 상점에 추가
        {
            AddItem(item);
        }
        AddItem(new Item(1,"수련자의 갑옷", 1000, 0, 5, ItemType.Armor, "수련에 도움을 주는 갑옷입니다."));
        AddItem(new Item(2,"무쇠갑옷", 2000, 0, 9, ItemType.Armor, "무쇠로 만들어져 튼튼한 갑옷입니다."));
        AddItem(new Item(3,"스파르타 갑옷", 3500, 0, 15, ItemType.Armor, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다."));
        AddItem(new Item(4,"낡은 검", 600, 2, 0, ItemType.Weapon, "쉽게 볼 수 있는 낡은 검 입니다."));
        AddItem(new Item(5,"청동 도끼", 1700, 5, 0, ItemType.Weapon, "어디선가 사용됐던거 같은 도끼입니다."));
        AddItem(new Item(6,"스파르타의 창", 2700, 7, 0, ItemType.Weapon, "스파르타의 전사들이 사용했다는 전설의 창입니다."));
        var sortList = items.OrderBy(x => x.id);
        items = sortList.ToList();
    }


    public void ShopMenu()//상점 메뉴
    {

        var sortList = items.OrderBy(x => x.id);
        items = sortList.ToList();
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
                inventory.ShowItmeList(items, false, true, true);
            }
            else if(isSaleList)//상점 판매 목록
            {
                Console.WriteLine("[판매 목록]");
                Console.WriteLine();
                inventory.ShowItmeList(inventory.ItemList, false, true, true);
            }
            else//상점 목록
            {
                Console.WriteLine("[상점 목록]");
                Console.WriteLine();
                inventory.ShowItmeList(items, false, true, false);
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

    public void AddItem(Item item)//아이템 추가
    {
        if (index.Contains(item.id) == false)
        {
            items.Add(item);
            index.Add(item.id);
        }
    }
}

public class Item //아이템 클래스
{
    public int id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int price { get; set; }
    public int attackPower { get; set; }
    public int defensePower { get; set; }
    public ItemType type { get; set; }
    public bool isEquip { get; set; }
  
    public bool isOwn { get; set; }

    public Item(int id, string name, int price, int attackPower, int defensePower, ItemType type, string description)
    {
    this.id = id;
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
            Console.WriteLine("0.게임종료");
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

    public void Rest()//휴식 메뉴
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
            if (int.TryParse(Console.ReadLine(), out select))//숫자를 입력받으면
            {
                Console.Clear();
                if (select == 1)
                {
                    Console.Clear();
                    if (character.gold >= 500)//골드가 500 이상일 때
                    {
                        if (character.hp == character.maxHp)//체력이 가득 찼을 때
                        {
                            Console.Clear();
                            Console.WriteLine("이미 체력이 가득 찼습니다.");
                        }
                        else if (character.hp < character.maxHp)// 체력이 가득 차지 않았을 때
                        {
                            character.hp = character.maxHp;
                            character.gold -= 500;
                            Console.Clear();
                            Console.WriteLine("휴식을 완료했습니다.");
                        }
                    }
                    else//골드가 500 미만일 때
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

public class Dungeon //던전 클래스
{
    private int difficultLevel;//던전 난이도
    public int dungeonCount = 0; //던전 클리어 횟수
    private int beforeHp = 0;//던전 입장 전 체력
    private int beforeGold = 0;//던전 입장 전 골드
    private bool isLive;//생존 여부
    Random random = new Random();
    private Character character;
    private Inventory inventory;

    public Dungeon(Character character, Inventory inventory)
    {
        this.character = character;
        this.inventory = inventory;
    }

    public void DungeonMenu()//던전 메뉴
    {
        isLive = true;
        Console.Clear();
        while (true)
        {
            Console.WriteLine("던전 입장");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1.쉬운 던전 | 방어력 5 이상 권장");
            Console.WriteLine("2.일반 던전 | 방어력 11 이상 권장");
            Console.WriteLine("3.어려운 던전 | 방어력 17 이상 권장");
            Console.WriteLine("0.나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해 주세요");
            Console.Write(">>");
            if (int.TryParse(Console.ReadLine(), out difficultLevel))//숫자를 입력받으면
            {
                switch (difficultLevel)
                {
                    case 0:
                        Console.Clear();
                        return;
                        break;
                    case 1:
                        StartDungeon(5);
                        break;
                    case 2:
                        StartDungeon(11);
                        break;
                    case 3:
                        StartDungeon(17);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
            if (!isLive)
            {
                break;
            }
        }
    }

    public void ClearDungeon(int difficult)//던전 클리어
    {
        int rewardGold;
        Console.Clear();
        Console.WriteLine("던전 클리어!");
        Console.WriteLine("축하합니다!!");
        dungeonCount++;
        if (dungeonCount >= character.level)//던전 클리어 횟수가 레벨과 같거나 많을 때 레벨업
        {
            dungeonCount = 0;
            character.LevelUp();
        }
        switch (difficult)
        {
            case 1:
                Console.WriteLine("쉬운 던전을 클리어 하셨습니다.");
                rewardGold = Reward(1000);
                break;
            case 2:
                Console.WriteLine("일반 던전을 클리어 하셨습니다.");
                rewardGold = Reward(1700);
                break;
            case 3:
                Console.WriteLine("어려운 던전을 클리어 하셨습니다.");
                rewardGold = Reward(2500);
                break;
            default:
                rewardGold = 0;
                break;
        }
        character.gold += rewardGold;

        Console.WriteLine();
        Console.WriteLine("체력 {0} -> {1}", beforeHp, character.hp);
        Console.WriteLine("Gold {0} G -> {1} G", beforeGold, character.gold);

        Console.WriteLine("");
    }

    private void PlayDungeon(int recommendateion)//던전 플레이
    {
        random.Next(20, 36);
        character.hp = character.hp - random.Next(20, 36) - recommendateion + character.defensePower;
        if(character.hp > 0)
        {
            ClearDungeon(difficultLevel);
        }
        else
        {
            inventory.RemoveAllItem();
            isLive = false;
        }
    }

    private void FailDungeon()//던전 실패
    {
        Console.Clear();
        Console.WriteLine("던전 실패...");
        character.hp = character.hp / 2;
    }

    private int Reward(int basicReward)//보상
    {
        int attack = (int)character.attackPower;
        return (basicReward * (100 + random.Next(attack, attack * 2 + 1))/100);
    }

    private void StartDungeon(int recommendation)
    {
        beforeHp = character.hp;
        beforeGold = character.gold;
        if (character.defensePower < recommendation)
        {
            if (random.Next(1, 101) <= 40)
            {
                FailDungeon();
            }
            else
            {
                PlayDungeon(recommendation);
            }
        }
        else
        {
            PlayDungeon(recommendation);
        }

    }
}

public class FileSaveLoad//파일 저장 및 불러오기
{
    string filePath = "Save.txt";
    Character character;
    Inventory inventory;
    Dungeon dungeon;

    public FileSaveLoad(Character character, Inventory inventory, Dungeon dungeon)
    {
        this.character = character;
        this.inventory = inventory;
        this.dungeon = dungeon;
    }
    public void Save()//파일 저장
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("level:_{0}", character.level);
            writer.WriteLine("name:_{0}", character.name);
            writer.WriteLine("cClass:_{0}", character.cClass);
            writer.WriteLine("baseAttackPower:_{0}", character.baseAttackPower);
            writer.WriteLine("baseDefensePower:_{0}", character.baseDefensePower);
            writer.WriteLine("maxHp:_{0}", character.maxHp);
            writer.WriteLine("hp:_{0}", character.hp);
            writer.WriteLine("gold:_{0}", character.gold);
            writer.WriteLine("dungeonCount:_{0}", dungeon.dungeonCount);
            writer.WriteLine("itemCount:_{0}", inventory.ItemList.Count);
            foreach (Item item in inventory.ItemList)
            {
                writer.WriteLine("id:_{0}", item.id);
                writer.WriteLine("name:_{0}", item.name);
                writer.WriteLine("price:_{0}", item.price);
                writer.WriteLine("attackPower:_{0}", item.attackPower);
                writer.WriteLine("defensePower:_{0}", item.defensePower);
                writer.WriteLine("type:_{0}", (int)item.type);
                writer.WriteLine("isEquip:_{0}", item.isEquip);
                writer.WriteLine("isOwn:_{0}", item.isOwn);
                writer.WriteLine("description:_{0}", item.description);
            }
        }
    }
    public void Load()//파일 불러오기
    {
        int id;
        string name;
        ItemType type;
        int price;
        int attackPower;
        int defensePower;
        bool isEquip;
        bool isOwn;
        string description;
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] words = line.Split('_');
                switch (words[0])
                {
                    case "level:":
                        character.level = int.Parse(words[1]);
                        break;
                    case "name:":
                        character.name = words[1];
                        break;
                    case "cClass:":
                        character.cClass = words[1];
                        break;
                    case "baseAttackPower:":
                        character.baseAttackPower = float.Parse(words[1]);
                        break;
                    case "baseDefensePower:":
                        character.baseDefensePower = int.Parse(words[1]);
                        break;
                    case "maxHp:":
                        character.maxHp = int.Parse(words[1]);
                        break;
                    case "hp:":
                        character.hp = int.Parse(words[1]);
                        break;
                    case "gold:":
                        character.gold = int.Parse(words[1]);
                        break;
                    case "dungeonCount:":
                        dungeon.dungeonCount = int.Parse(words[1]);
                        break;
                    case "itemCount:":
                        int count = int.Parse(words[1]);
                        for (int i = 0; i < count; i++)
                        {
                            id = int.Parse(lines[9 * i + 10].Split('_')[1]);
                            name = lines[9 * i + 11].Split('_')[1];
                            price = int.Parse(lines[9 * i + 12].Split('_')[1]);
                            attackPower = int.Parse(lines[9 * i + 13].Split('_')[1]);
                            defensePower = int.Parse(lines[9 * i + 14].Split('_')[1]);
                            type = (ItemType)Enum.Parse(typeof(ItemType), lines[9 * i + 15].Split('_')[1]);
                            isEquip = bool.Parse(lines[9 * i + 16].Split('_')[1]);
                            isOwn = bool.Parse(lines[9 * i + 17].Split('_')[1]);
                            description = lines[9 * i + 18].Split('_')[1];
                            Item item = new Item(id, name, price, attackPower, defensePower, type, description);
                            item.isEquip = isEquip;
                            item.isOwn = isOwn;
                            inventory.AddItem(item);
                        }
                        break;
                    
                }
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
        Dungeon dungeon = new Dungeon(character, inventory);
        FileSaveLoad fileSaveLoad = new FileSaveLoad(character, inventory, dungeon);
        int select = 0;
        bool isPlaying = true; //게임이 진행 중인지 확인
        if (File.Exists("Save.txt"))
        {
            fileSaveLoad.Load();
        }
        Shop shop = new Shop(inventory);

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
                    dungeon.DungeonMenu();//던전 메뉴를 보여줌
                    break;
                case 5:
                    village.Rest();//휴식 메뉴를 보여줌
                    break;
                case 0:
                    isPlaying = false;//0을 입력하면 게임 종료
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
            fileSaveLoad.Save();
        }
    }
}
