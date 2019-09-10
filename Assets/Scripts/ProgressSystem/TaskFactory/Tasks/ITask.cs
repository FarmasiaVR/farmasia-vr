public interface ITask {


    void NextTask();
    void FinishTask();
    string GetDescription();
    string GetHint();
}
