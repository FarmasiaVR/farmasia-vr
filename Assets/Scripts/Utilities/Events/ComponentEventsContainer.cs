using System.Collections.Generic;

public class ComponentEventsContainer {

    public List<CallbackComponentPair> pairs;

    public ComponentEventsContainer() {
        pairs = new List<CallbackComponentPair>();
    }

    public void AddPair(CallbackComponentPair pair) {
        pairs.Add(pair);
    }

    /// <summary>
    /// Calls callbacks if the component which subscribed to it isn't null. Automatically removes subscriptions for destroyed components/gameobjects.
    /// </summary>
    public void FireIfNotNull(CallbackData data) {
        for (int i = 0; i < pairs.Count; i++) {
            CallbackComponentPair pair = pairs[i];

            if (pair.Component == null) {
                pairs.RemoveAt(i);
                i--;
                continue;
            }

            pair.CallbackWithData(data);
        }
    }

    public void Unsubscribe(CallbackComponentPair pair) {
        for (int i = 0; i < pairs.Count; i++) {
            if (pair.Equals(pairs[i])) {
                pairs.RemoveAt(i);
                i--;
                return;
            }
        }
    }
}
