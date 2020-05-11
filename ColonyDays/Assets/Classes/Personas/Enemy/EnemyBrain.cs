/// <summary>
/// An Enemy will only have a MilitarBrain
public class EnemyBrain : MilitarBrain
{
    private Structure _storage;

    public EnemyBrain(Person person)
    {
        _person = person;
        _storage = BuildingController.FindTheClosestOfContainTypeFullyBuilt("Storage", _person.transform.position);

        SetMyDummyToMyCurrPosAndIni();
        _fin = _storage;

        CreateRoute();
        GameController.ChangeGameModeTo(H.War);
    }

    public void Update()
    {
        base.Update();
    }

    private void CreateRoute()
    {
        _cryRouteManager = new CryRouteManager(_ini, _fin, _person, iniDoor: false, finDoor: false);
    }
}