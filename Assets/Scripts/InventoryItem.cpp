// write an inventoryItem class for Unity for a 3D game. The item needs to be able to be picked up by the player using a collision and is used by another class called InventoryManager. Some of the items will be quest items, but the others will just be collectables that would be counted?

class InventoryItem
{
public:
    InventoryItem();
    ~InventoryItem();

    void PickUp();
    void Drop();

    bool IsPickedUp();
    bool IsDropped();

private:
    bool m_isPickedUp;
    bool m_isDropped;
};