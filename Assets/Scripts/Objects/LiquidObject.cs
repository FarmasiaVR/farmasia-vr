﻿using System;
using System.Collections;
using UnityEngine;

public struct LiquidScale {
    public int minLinearAmount;
    public int maxLinearAmount;
    public float minLinearFloatValue;
}

public class LiquidObject : MonoBehaviour {
    private enum Capacity {
        None,
        _50ml,
        _25ml,
        _5ml,
        _1ml,
        _2ml,
        _03ml,
    }

    #region fields
    private LiquidScale liquidScale;

    [SerializeField]
    private Capacity capacity;

    [SerializeField]
    private MeshRenderer mesh;

    [SerializeField]
    private bool HasRealLiquidMaterial;

    [SerializeField]
    public float percentage;

    #endregion

    void Awake() {
        if (HasRealLiquidMaterial) {
            InitObject();
        } else {
            UpdateObject();
        }

        if (capacity == Capacity._50ml) {
            liquidScale = new LiquidScale {
                minLinearAmount = 4000,
                maxLinearAmount = 50000,
                minLinearFloatValue = 0.144f
            };
        }
        else if (capacity == Capacity._25ml) {
            liquidScale = new LiquidScale {
                minLinearAmount = 4000,
                maxLinearAmount = 25000,
                minLinearFloatValue = 0.2f
            };
        }
        else if (capacity == Capacity._1ml) {
            liquidScale = new LiquidScale {
                minLinearAmount = 100,
                maxLinearAmount = 1000,
                minLinearFloatValue = 0.145f
            };
        }
        else if (capacity == Capacity._5ml) {
            liquidScale = new LiquidScale {
                minLinearAmount = 500,
                maxLinearAmount = 5000,
                minLinearFloatValue = 0.145f
            };
        }
        else if (capacity == Capacity._2ml) {
            liquidScale = new LiquidScale {
                minLinearAmount = 100,
                maxLinearAmount = 2000,
                minLinearFloatValue = 0.145f
            };
        }
        else if (capacity == Capacity._03ml) {
            liquidScale = new LiquidScale {
                minLinearAmount = 100,
                maxLinearAmount = 300,
                minLinearFloatValue = 0.4f
            };
        }
    }

    // OnValidate sucks, don't use it
    //private void OnValidate() {
    //    UpdateObject();
    //}

    public void SetFillPercentage(float percentage, LiquidContainer container) {

        if (float.IsNaN(percentage)) {
            throw new Exception("Value was NaN");
        }

        if (percentage < 0 || percentage > 1) {
            throw new ArgumentOutOfRangeException("percentage", percentage.ToString(), "Percentage should be [0, 1]");
        }

        this.percentage = percentage;

        UpdateObject(container.Amount);
    }

    private void InitObject() {
        mesh.material.SetFloat("_Fill", percentage);
    }

    public void UpdateObject(int? amount = null) {
        if (!HasRealLiquidMaterial) {
            // localScale scales around pivot (default is center of object)
            // Therefore, translation needed


            transform.localScale = new Vector3(1, percentage, 1);

            // Translate by scale delta amount
            float newY = percentage - 1;
            transform.localPosition = new Vector3(0, newY, 0);
        } else {
            float updateAmount = percentage;
            if (capacity != Capacity.None && amount != null && percentage != amount) {
                if (amount < liquidScale.minLinearAmount) {
                    updateAmount = (float) amount / liquidScale.minLinearAmount * liquidScale.minLinearFloatValue;
                } else {
                    updateAmount = liquidScale.minLinearFloatValue +
                                   (float) (amount - liquidScale.minLinearAmount) / (liquidScale.maxLinearAmount - liquidScale.minLinearAmount) *
                                   (1 - liquidScale.minLinearFloatValue);
                }
            }
            StartCoroutine(LerpLiquid(updateAmount, 0.5f));
        }
    }

    private IEnumerator LerpLiquid(float targetAmount, float lerpTimeInSeconds) {
        if (lerpTimeInSeconds == 0) yield break;
        
        float t = 0;
        float startAmount = mesh.material.GetFloat("_Fill");
        float currentAmount;

        if (mesh != null && targetAmount != 0) {
            mesh.enabled = true;
        }

        while (t < lerpTimeInSeconds) {
            currentAmount = Mathf.Lerp(startAmount, targetAmount, t / lerpTimeInSeconds);
            t += Time.deltaTime;

            mesh.material.SetFloat("_Fill", currentAmount);

            yield return null;
        }

        if (mesh != null && targetAmount == 0) {
            mesh.enabled = false;
        }
    }

    public void SetMaterialFromType(LiquidType type) {
        switch (type) {
            case LiquidType.Peptonwater or LiquidType.PhosphateBuffer: // Stole pepton water texture for phosphate buffer since it just looks the same lol -Grp6
                if (HasRealLiquidMaterial) {
                    mesh.material.SetColor("_SideColor", new Color(0.3722854f, 0.8229616f, 0.8867924f, 1));
                    mesh.material.SetColor("_TopColor", new Color(0.2722854f, 0.8229616f, 0.8867924f, 1));
                } else {
                    mesh.material = Resources.Load<Material>("Liquids/PeptonWater");
                }
                break;
                
            case LiquidType.Tioglygolate:
                if (HasRealLiquidMaterial) {
                    mesh.material.SetColor("_SideColor", new Color(0.802083f, 0.382115f, 0.170352f, 1));
                    mesh.material.SetColor("_TopColor", new Color(0.802083f, 0.382115f, 0.170352f, 1));
                } else {
                    mesh.material = Resources.Load<Material>("Liquids/Tioglygolate");
                }
                break;
                
            case LiquidType.Soycaseine:
                if (HasRealLiquidMaterial) {
                    mesh.material.SetColor("_SideColor", new Color(0.9528301f, 0.5856799f, 0.215735f, 1));
                    mesh.material.SetColor("_TopColor", new Color(0.9528301f, 0.5856799f, 0.215735f, 1));
                } else {
                    mesh.material = Resources.Load<Material>("Liquids/Soycaseine"); ;
                }
                break;

            case LiquidType.MeatSoup:
                if (HasRealLiquidMaterial) {
                    mesh.material.SetColor("_SideColor", new Color(1f, 0.7570499f, 0.1603773f, 1));
                    mesh.material.SetColor("_TopColor", new Color(1f, 0.7570499f, 0.1603773f, 1));
                }
                else {
                    mesh.material = Resources.Load<Material>("Liquids/MeatSoup");
                }
                break;
                
            case LiquidType.Virkon:
                if (HasRealLiquidMaterial) {
                    mesh.material.SetColor("_SideColor", new Color(1f, 0.1607f, 0.5749f, 1));
                    mesh.material.SetColor("_TopColor", new Color(1f, 0.1607f, 0.5749f, 1));
                }
                else {
                    mesh.material = Resources.Load<Material>("Liquids/Virkon1%");
                }
                break;

            case LiquidType.Senna01:
                if (HasRealLiquidMaterial) {
                    mesh.material.SetColor("_SideColor", new Color(0.8509804f, 0.9849057f, 0.0f, 0.7254902f));
                    mesh.material.SetColor("_TopColor", new Color(0.8509804f, 0.9849057f, 0.0f, 0.7254902f));
                } else {
                    mesh.material = Resources.Load<Material>("Liquids/Senna01");
                }
            break;

            case LiquidType.Senna01m:
                if (HasRealLiquidMaterial) {
                    mesh.material.SetColor("_SideColor", new Color(1f, 0.04f, 0.0f, 1));
                    mesh.material.SetColor("_TopColor", new Color(1f, 0.04f, 0.0f, 1));
                } else {
                    mesh.material = Resources.Load<Material>("Liquids/Senna01m");
                }
            break;

            case LiquidType.Senna001:
                if (HasRealLiquidMaterial) {
                    mesh.material.SetColor("_SideColor", new Color(0.8509804f, 0.9849057f, 0.0f, 0.4901961f));
                    mesh.material.SetColor("_TopColor", new Color(0.8509804f, 0.9849057f, 0.0f, 0.4901961f));
                } else {
                    mesh.material = Resources.Load<Material>("Liquids/Senna001");
                }
                break;
            
            case LiquidType.Senna001m:
                if (HasRealLiquidMaterial) {
                    mesh.material.SetColor("_SideColor", new Color(1f, 0.04f, 0.0f, 1));
                    mesh.material.SetColor("_TopColor", new Color(1f, 0.04f, 0.0f, 1));
                } else {
                    mesh.material = Resources.Load<Material>("Liquids/Senna001m");
                }
                break;

            case LiquidType.Senna0001:
                if (HasRealLiquidMaterial) {
                    mesh.material.SetColor("_SideColor", new Color(0.8509804f, 0.9849057f, 0.0f, 0.254902f));
                    mesh.material.SetColor("_TopColor", new Color(0.8509804f, 0.9849057f, 0.0f, 0.254902f));
                } else {
                    mesh.material = Resources.Load<Material>("Liquids/Senna001");
                }
                break;
            
            case LiquidType.Senna0001m:
                if (HasRealLiquidMaterial) {
                    mesh.material.SetColor("_SideColor", new Color(1f, 0.04f, 0.0f, 1));
                    mesh.material.SetColor("_TopColor", new Color(1f, 0.04f, 0.0f, 1));
                } else {
                    mesh.material = Resources.Load<Material>("Liquids/Senna001m");
                }
                break;
            
                case LiquidType.Senna1:
                if (HasRealLiquidMaterial) {
                    mesh.material.SetColor("_SideColor", new Color(0.8509804f, 0.9849057f, 0.0f, 0.8407843f));
                    mesh.material.SetColor("_TopColor", new Color(0.8509804f, 0.9849057f, 0.0f, 0.8407843f));
                }
                else {
                    mesh.material = Resources.Load<Material>("Liquids/Senna1");
                }
                break;

                case LiquidType.Senna1m:
                if (HasRealLiquidMaterial) {
                    mesh.material.SetColor("_SideColor", new Color(1f, 0.04f, 0.0f, 1));
                    mesh.material.SetColor("_TopColor", new Color(1f, 0.04f, 0.0f, 1));
                }
                else {
                    mesh.material = Resources.Load<Material>("Liquids/Senna1m");
                }
                break;

            case LiquidType.SennaPowder:
                if (HasRealLiquidMaterial) {
                    mesh.material.SetColor("_SideColor", new Color(0.8509804f, 0.9849057f, 0.0f, 0.9607843f));
                    mesh.material.SetColor("_TopColor", new Color(0.8509804f, 0.9849057f, 0.0f, 0.9607843f));
                }
                else {
                    mesh.material = Resources.Load<Material>("Liquids/SennaPowder");
                }
                break;
                

            case LiquidType.None:
                if (HasRealLiquidMaterial) {
                    mesh.material.SetColor("_SideColor", new Color(0.6818f, 0.8617f, 0.8867f, 0.3882f));
                    mesh.material.SetColor("_TopColor", new Color(0.6818f, 0.8617f, 0.8867f, 0.3882f));
                }
                else {
                    mesh.material = Resources.Load<Material>("Liquids/ClearLiquid");
                }
                break;
        }
    }
}        
