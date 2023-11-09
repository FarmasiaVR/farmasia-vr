public interface ITogglableFire {
    bool isBurning {get;}

    void Extinguish();
    void Ignite();
}