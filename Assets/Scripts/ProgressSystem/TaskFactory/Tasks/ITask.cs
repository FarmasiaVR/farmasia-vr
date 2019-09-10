public interface ITask {

    

    void trigger();
    void finishTask();
    string getDescription();
    string getHint();
}
