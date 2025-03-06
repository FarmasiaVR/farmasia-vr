using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MixingManager : MonoBehaviour {
    public UnityEvent<string, int> onMistake;
    public static Dictionary<Tuple<LiquidType, LiquidType>, LiquidType> recipes = new()
    {
        { Tuple.Create(LiquidType.SennaPowder,LiquidType.PhosphateBuffer), LiquidType.Senna1m },
        { Tuple.Create(LiquidType.Senna1m,LiquidType.PhosphateBuffer), LiquidType.Senna1m },
        { Tuple.Create(LiquidType.PhosphateBuffer,LiquidType.Senna1), LiquidType.Senna01m },
        { Tuple.Create(LiquidType.Senna01m,LiquidType.Senna1), LiquidType.Senna01m },                
        { Tuple.Create(LiquidType.PhosphateBuffer, LiquidType.Senna01), LiquidType.Senna001m },
        { Tuple.Create(LiquidType.Senna001m, LiquidType.Senna01), LiquidType.Senna001m },
        { Tuple.Create(LiquidType.PhosphateBuffer, LiquidType.Senna001), LiquidType.Senna0001m },
        { Tuple.Create(LiquidType.Senna0001m, LiquidType.Senna001), LiquidType.Senna0001m }        
    };

    public static Dictionary<LiquidType, Tuple<int, int>> minMaxMixingValue = new()
    {
        { LiquidType.SennaPowder, Tuple.Create(1000, 1500) },
        { LiquidType.Senna1m, Tuple.Create(500, 6000) },
        { LiquidType.Senna1, Tuple.Create(500, 6000) },
        { LiquidType.PhosphateBuffer, Tuple.Create(4500, 5000) },
        { LiquidType.Senna01m, Tuple.Create(500, 5000) },
        { LiquidType.Senna001m, Tuple.Create(4500, 5000) },
        { LiquidType.Senna0001m, Tuple.Create(4500, 5000) }
    };

    public bool checkControlAmount(LiquidContainer source, LiquidContainer target) {
        if (target.LiquidType == LiquidType.PhosphateBuffer && target.amount==1000
        && source.LiquidType!=LiquidType.PhosphateBuffer){
            return false;
        }
        return true;
    }

    public bool Dilution(LiquidContainer source, LiquidContainer target, int transferAmount){        
        Debug.Log("Trying to mix: " + source.LiquidType + " with " + target.LiquidType);

        var key = Tuple.Create(target.LiquidType, source.LiquidType);

        if (checkControlAmount(source, target)==false) {
            onMistake.Invoke("Dont'mix liquids in the control tube.",1);
            return false;
        }

        if (recipes.TryGetValue(key, out LiquidType newResult))
        {   
            if (minMaxMixingValue.TryGetValue(target.LiquidType, out var tupleValue)){
                var (min, max) = tupleValue;
                if (min <= target.Amount+transferAmount && target.Amount+transferAmount <= max){
                    if(target.LiquidType!= newResult) target.mixingValue = 0;
                    target.LiquidType = newResult;
                    target.SetLiquidMaterial();
                        // Apply the new result
                    
                    Debug.Log("New LiquidType: " + newResult);
                    return true;
                }
                else{
                    onMistake.Invoke("The amounts of the liquids are incorrect.",1);
                    return false;
                }
            }
            else{
                onMistake.Invoke("The amounts of the liquids are incorrect.",1);
                return false;
            }

        } else {          
            onMistake.Invoke($"Mixing failed: Are you sure these are the correct liquids?",1);
            return false;
        }
    }
}