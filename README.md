# Система инвентаря с drag & drop

## - Инвентарь
Логика и UI разделены на следующие скрипты, Inventory.cs и InventoryUI.cs соответственно.

Настройки InventoryUI
> Возможность устанавливать количество слотов. Например: 5x4. <br>
> Добавлять новые виды сортировки используя enum вместо хард-кода

<img width="654" height="366" alt="{88A0C567-BB77-43D8-AC7B-1652EA462A7E}" src="https://github.com/user-attachments/assets/e37c7944-9c9f-41b9-b714-aca2d6b25d6a" />

## - Предметы
Все предметы реализованы через ScriptableObject

```cs
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
    public class ItemData : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
        public string description;
        public ItemType type;
        public bool isStackable = false;
    }
```

Пример настройки предмета

<img width="657" height="188" alt="{6F1DFB48-4DC8-4AB0-AE5C-4C131CAAAE61}" src="https://github.com/user-attachments/assets/794bbca6-c5de-409c-8a71-adeee20703aa" />


## - Функционал

> Работа системы инвентаря

![DragAndDrop](https://github.com/user-attachments/assets/9b7248db-6a20-49bc-b93e-66205f7931f9)

## - Руководство пользователя
[Скачать проект](https://github.com/Nuk1o/DragAndDrop/releases/tag/release)
* Управление:
  * Стандартное управление в Drag&Drop (ЛКМ)
  * Для использования предмета следует сделать дабл-клик (ЛКМ x2)
  * Для открытия информативного окна следует нажать один раз на предмет (ЛКМ)
  * Выброс предметов происходит при нажатии красной стрелки <img width="77" height="81" alt="{7D21ACB3-AB54-422C-8DA0-A5106C8DC475}" src="https://github.com/user-attachments/assets/bf8f2650-0cff-4a92-abc9-d09ca056aec7" />
  * Выбор сортировки (находится сверху справа) <img width="182" height="95" alt="{45C18344-5391-41F7-99AA-EF5545249534}" src="https://github.com/user-attachments/assets/a0688069-7b59-4680-89b5-8eab27dc750c" />
  * Для того чтобы стекнуть предмет, поместите его в точно такой же предмет


