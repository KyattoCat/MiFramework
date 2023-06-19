using System.Collections;
using MiFramework.AI.GOAP;
public static class Program
{
    public static void Main(string[] args)
    {
        MainLoop();
    }

    public static void MainLoop()
    {
        GOAPTest test = new GOAPTest();
        while (true)
        {
            test.Update();
        }
    }
}

public class OpenBedRoomDoor : GAction
{
    public OpenBedRoomDoor()
    {
        effects.Add("BedRoomDoorOpen", new EffectItem { op = Op.SET, value = 1 });
    }
}

public class GoToBedRoom : GAction
{
    public GoToBedRoom()
    {
        conditions.Add("BedRoomDoorOpen", new ConditionItem { comparisonOp = ComparisonOp.GE, value = 1});
        effects.Add("InBedRoom", new EffectItem { op = Op.SET, value = 1 });
    }

    public override void PreProcess(GAgent agent)
    {
        base.PreProcess(agent);
        Console.WriteLine("need sleep");
    }

    public override GActionState Process(GAgent agent)
    {
        base.Process(agent);
        Console.WriteLine("go to bedroom");
        return GActionState.Success;
    }

    public override void PostProcess(GAgent agent)
    {
        base.PostProcess(agent);
        Console.WriteLine("I'm in");
    }
}

public class Sleep : GAction
{
    public override bool SupportLoop => true;

    public Sleep()
    {
        conditions.Add("InBedRoom", new ConditionItem(ComparisonOp.EQ, 1));
        effects.Add("Stamina", new EffectItem(Op.ADD, 50));
    }

    public override GActionState Process(GAgent agent)
    {
        Console.WriteLine("Zzz");
        return GActionState.Success;
    }
}

public class StaminaGoal : GGoal
{
    public StaminaGoal(ComparisonOp comparisonOp, int stamina) : base("Stamina", comparisonOp, stamina)
    {

    }
}

public class GOAPTest
{
    private GAgent agent;

    public GOAPTest()
    {
        agent = new GAgent();

        agent.SetState("Stamina", 0);

        agent.SetGoal(new StaminaGoal(ComparisonOp.GE, 200));

        agent.AddAction<GoToBedRoom>();
        agent.AddAction<OpenBedRoomDoor>();
        agent.AddAction<Sleep>();
    }

    public void Update()
    {
        agent.Update();
    }
}