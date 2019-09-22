using System.Collections.Generic;

public class PipelineManager {

    #region fields
    private List<Pipeline> pipelines;
    #endregion

    public PipelineManager() {
        pipelines = new List<Pipeline>();
    }

    public Pipeline New() {
        Pipeline p = new Pipeline();
        pipelines.Add(p);
        return p;
    }

    public void Add(Pipeline pipeline) {
        pipelines.Add(pipeline);
    }

    public void Update(float deltaTime) {
        for (int i = pipelines.Count - 1; i >= 0; i--) {
            pipelines[i].Update(deltaTime);
            if (pipelines[i].IsDone) {
                pipelines.RemoveAt(i);
            }
        }
    }
}