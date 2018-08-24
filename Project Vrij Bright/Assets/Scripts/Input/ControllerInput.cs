public class ControllerInput
{
    public virtual bool Trig_CheckInput()
    {
        return false;
    }
    public virtual bool A_CheckInput()
    {
        return false;
    }
    public virtual bool B_CheckInput()
    {
        return false;
    }
    public virtual bool X_CheckInput()
    {
        return false;
    }
    public virtual bool Y_CheckInput()
    {
        return false;
    }

    public virtual string GetControllerName()
    {
        return "";
    }

    public virtual string GetVertical()
    {
        return "";
    }
    public virtual string GetHorizontal()
    {
        return "";
    }
}
