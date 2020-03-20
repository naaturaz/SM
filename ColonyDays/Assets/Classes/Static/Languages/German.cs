using System.Collections.Generic;

public class German
{
    private static LangDict _de = new LangDict();

    /// </summary>
    public static void ReloadDict()
    {
        string _houseTail = "Hier lebt er nun, der SugarMiller und genießt wenigstens ab und zu eine gute Mahlzeit";
        string _animalFarmTail = ", In diesem Gebäude können verschiedene Arten von Tieren gezüchtet werden";
        string _fieldFarmTail = ", In diesem Gebäude können verschiedene Pflanzen und Früchte angebaut werden";
        string _asLongHasInput = ", solange die notwendigen Komponenten verfügbar sind";
        string _produce = "In diesem Gebäude wird von den Arbeitern das ausgewählte Produkt hergestellt, sofern die erforderlichen Materialien verfügbar sind";
        string _storage =
        "Dies ist ein Lagergebäude. Wenn es voll wird, arbeiten die Leute nicht mehr, da sie nichts haben, wo sie ihre Produkte lagern können";
        string _militar = "Dieses Gebäude trägt dazu bei, die Piratenbedrohung in Ihrem Hafen zu verringern. Um effektiv zu funktionieren, sollten sich Arbeiter im Inneren befinden. Je mehr Arbeiter, desto besser";
        string _notRegionNeeded = "Kann gebaut werden, ohne die Region zu besitzen.";

        _de = new LangDict();


            //Descriptions
            //Infr
        _de.Add("Road.Desc","Dies dient zu Dekorationszwecken. Die Menschen sind schlichtweg zufriedener, wenn sie Straßen vorfinden");
        _de.Add("BridgeTrail.Desc","Erlaubt es den Menschen, von einer Seite des Ufers zur anderen zu gelangen ohne nasse Füße zu bekommen");
        _de.Add("BridgeRoad.Desc","Erlaubt es den Menschen, von einer Seite des Ufers zur anderen zu gelangen. Die Menschen lieben diese Brücken. Sie geben ihnen ein Gefühl von Wohlstand und Glück" +_houseTail);
        _de.Add("LightHouse.Desc","Hilft dabei, die Sichtbarkeit des Hafens zu erhöhen. Der Hafen verfügt damit über ein Ansehen, unter der Bedingung dass hier Arbeiter tätig sind");
        _de.Add(H.Masonry + ".Desc","Unabdingbares Gebäude. Die hier beschäftigten Arbeiter bauen neue Gebäude und arbeiten ausserdem als Transporteure, wenn es mal nichts zu tun gibt");
        _de.Add(H.StandLamp + ".Desc","Beleuchtet nachs die Strassen, wenn Walöl im Lager der Stadt verfügbar ist");

        _de.Add(H.HeavyLoad + ".Desc","Diese Arbeiter benutzen Pferdefuhrwerke, um Waren zu transportieren");


            //House
        _de.Add("Bohio.Desc", "Das Bohiohaus, primitive Bedingungen, welche die Menschen unzufrieden lassen. Maximal finden hier 2-3 Kinder Platz");

        _de.Add("Shack.Desc", "Die Hütte, primitive Verhältnisse mit unzufriedenen Menschen, die maximal 2 Kinder haben können");
        _de.Add("MediumShack.Desc", "Die mittelgroße Hütte, mit primitiven Bedingungen für ein wenig mehr Zufriedenheit, Hier haben 2-3 Kinder ein Zuhause");
        _de.Add("LargeShack.Desc", "Die große Hütte, bietet etwas bessere Bedingungen. Die Menschen sind zufriedener und sie bietet Platz für 2-4 Kinder");


        _de.Add("WoodHouseA.Desc", "Mittelgroßes Holzhaus, eine Familie kann maximal 2-3 Kinder haben" );
        _de.Add("WoodHouseB.Desc", "Mittelgroßes Holzhaus, eine Familie kann maximal 3-4 Kinder haben"  );
        _de.Add("WoodHouseC.Desc", "Mittelgroßes Holzhaus, eine Familie kann maximal 2-3 Kinder haben");
        _de.Add("BrickHouseA.Desc", "Mittleres Haus, eine Familie kann maximal 3 Kinder haben");
        _de.Add("BrickHouseB.Desc","Großes Haus, eine Familie kann maximal 3-4 Kinder haben");
        _de.Add("BrickHouseC.Desc","Großes Haus, eine Familie kann maximal 4 Kinder haben");

            
            //Farms
            //Animal
        _de.Add("AnimalFarmSmall.Desc","Kleine Tierfarm"+_animalFarmTail);
        _de.Add("AnimalFarmMed.Desc","Mittlere Tierfarm"+_animalFarmTail);
        _de.Add("AnimalFarmLarge.Desc","Große Tierfarm"+_animalFarmTail);
        _de.Add("AnimalFarmXLarge.Desc","Riesige Tierfarm"+_animalFarmTail);
            //Fields
        _de.Add("FieldFarmSmall.Desc","Kleine Feldfarm"+_fieldFarmTail);
        _de.Add("FieldFarmMed.Desc","Mittlere Feldfarm"+_fieldFarmTail);
        _de.Add("FieldFarmLarge.Desc","Große Feldfarm"+_fieldFarmTail);
        _de.Add("FieldFarmXLarge.Desc","Riesige Feldfarm"+_fieldFarmTail);
        _de.Add(H.FishingHut + ".Desc","Mit diesem Gebäude kann ein Arbeiter in einem Fluss Fische fangen (muss an einem Fluss plaziert werden)." + _notRegionNeeded);

            //Raw
        _de.Add("Mortar.Desc","In diesem Gebäude ensteht Mörtel, der für Bauvorhaben benötigt wird");
        _de.Add("Clay.Desc","Dieses Gebäude stellt Ton her. Dieser dient als Rohmaterial für Ziegel und mehr");
        _de.Add("Pottery.Desc","In diesem Gebäude entstehen Keramikprodukte wie Geschirr, Gläser usw.");
        _de.Add("Mine.Desc","Dieses Gebäude dient dem Bergbau");
        _de.Add("MountainMine.Desc","Dieses Gebäude dient der Förderung von Erz");
        _de.Add("Resin.Desc","Minenarbeiter sind hier tätig. Bei der Arbeit ist es möglich dass sie zufällige Mineralien und Metalle gewinnen.");
        _de.Add( H.LumberMill +".Desc","Hier finden Arbeiter Ressourcen wie Holz, Stein und Erz");
        _de.Add("BlackSmith.Desc","Dieses Gebäude dient der Herstellung verschiedener Dinge"+_asLongHasInput);
        _de.Add("ShoreMine.Desc","In diesem Gebäude werden Salz und Sand hergestellt");
        _de.Add("QuickLime.Desc","Dieses Gebäude produziert ungelöschten Kalk");

            //Prod
        _de.Add("Brick.Desc","Dieses Gebäude stellt Produkte aus Lehm her, wie Ziegelsteine usw.");
        _de.Add("Carpentry.Desc","Dieses Gebäude stellt Holzprodukte her, wie Kisten, Fässer usw.");
        _de.Add("Cigars.Desc","Dieses Gebäude dient der Herstellung von Zigarren"+_asLongHasInput);
        _de.Add("Mill.Desc","In diesem Gebäude wird Korn gemahlen"+_asLongHasInput);
        _de.Add(H.Tailor+".Desc","Dieses Gebäude dient der Herstellung von Kleidung"+_asLongHasInput);
        _de.Add("Tilery.Desc","Dieses Gebäude dient der Herstellung von Dachziegeln"+_asLongHasInput);
        _de.Add("Armory.Desc","Dieses Gebäude dient der Herstellung von Waffen"+_asLongHasInput);
        _de.Add(H.Distillery+".Desc",_produce);
        _de.Add("Chocolate.Desc",_produce);
        _de.Add("Ink.Desc",_produce);

            //Ind
        _de.Add("Cloth.Desc",_produce);
        _de.Add("GunPowder.Desc",_produce);
        _de.Add("PaperMill.Desc",_produce);
        _de.Add("Printer.Desc",_produce);
        _de.Add("CoinStamp.Desc",_produce);
        _de.Add("Silk.Desc",_produce);
        _de.Add("SugarMill.Desc",_produce);
        _de.Add("Foundry.Desc",_produce);
        _de.Add("SugarShop.Desc", "Stellt Produkte aus Zucker her!!!. " + _produce);


            _de.Add("SteelFoundry.Desc",_produce);

            //trade
        _de.Add("Dock.Desc","Hier kannst du Import- oder Exportaufträge hinzufügen (muss am Meer platziert werden)." + _notRegionNeeded);
        _de.Add(H.Shipyard + ".Desc","Hier lassen sich Schiffe reparieren, aber achte darauf dass die nötigen Reparaturmaterialien im Bestand sind");
        _de.Add("Supplier.Desc","Hier werden Schiffe mit Waren be- und entladen, aber es müssen Artikel im Inventar sein, die auf dem Schiff für die lange Fahrt verwendet werden");
        _de.Add("StorageSmall.Desc",_storage);
        _de.Add("StorageMed.Desc",_storage);
        _de.Add("StorageBig.Desc",_storage);
        _de.Add("StorageBigTwoDoors.Desc",_storage);
        _de.Add("StorageExtraBig.Desc",_storage);

            //gov
        _de.Add("Library.Desc","Die Leute kommen in dieses Gebäude, um Bücher zu lesen oder sie zu leihen um ihr Wissen zu erweitern. Je mehr Inventar in den Bibliotheken, desto besser");
        _de.Add("School.Desc","Hier erhalten die Leute eine Ausbildung. Hier gilt mehr ist besser");
        _de.Add("TradesSchool.Desc","Hier erhalten die Menschen eine spezialisierte Ausbildung im Handwerk. Gut ein paar Spezialisten zu haben");
        _de.Add("TownHouse.Desc","Das Stadthaus erhöht die Zufriedenheit und den Wohlstand Ihres Volkes");

            //other
        _de.Add("Church.Desc","Die Kirche gibt deinem Volk Glück und Hoffnung");
        _de.Add("Tavern.Desc","Die Taverne bietet deinen Leuten Entspannung und Unterhaltung");

            //Militar
        _de.Add("WoodPost.Desc", "Entdeckt Banditen und Piraten schneller, so dass du dich im Voraus darauf vorbereiten können");
        _de.Add("PostGuard.Desc",_militar);
        _de.Add("Fort.Desc",_militar);
        _de.Add("Morro.Desc",_militar+". Sobald Sie dies gebaut haben, sollten Piraten es besser wissen");

            //Decoration
        _de.Add("Fountain.Desc", "Verschönert deine Stadt und erhöht bei deinen Bürgern die allgemeine Zufriedenheit");
        _de.Add("WideFountain.Desc", "Verschönert deine Stadt und erhöht bei deinen Bürgern die allgemeine Zufriedenheit");
        _de.Add("PalmTree.Desc", "Verschönert deine Stadt und erhöht bei deinen Bürgern die allgemeine Zufriedenheit");
        _de.Add("FloorFountain.Desc", "Verschönert deine Stadt und erhöht bei deinen Bürgern die allgemeine Zufriedenheit");
        _de.Add("FlowerPot.Desc", "Verschönert deine Stadt und erhöht bei deinen Bürgern die allgemeine Zufriedenheit");
        _de.Add("PradoLion.Desc", "Verschönert deine Stadt und erhöht bei deinen Bürgern die allgemeine Zufriedenheit");



            //Buildings name
            //Infr
        _de.Add("Road","Straße");
        _de.Add("BridgeTrail","Fußgängerbrücke");
        _de.Add("BridgeRoad","Straßenbrücke");
        _de.Add("LightHouse","Leuchtturm");
        _de.Add("Masonry","Maurerbetrieb");
        _de.Add("StandLamp","Straßenlaterne");
        _de.Add("HeavyLoad","Schwerlastbetrieb");


            //House
        _de.Add("Shack", "Hütte");
        _de.Add("MediumShack", "Mittelgroße Hütte");
        _de.Add("LargeShack", "Große Hütte");

        _de.Add("WoodHouseA", "Mittelgroßes Holzhaus" );
        _de.Add("WoodHouseB", "Großes Holzhaus"  );
        _de.Add("WoodHouseC", "Luxuriöses Holzhaus");
        _de.Add("BrickHouseA", "Mittelgroßes Ziegelhaus");
        _de.Add("BrickHouseB","Luxuriöses Ziegelhaus");
        _de.Add("BrickHouseC","Großes Ziegelhaus");

            
            //Farms
            //Animal
        _de.Add("AnimalFarmSmall","Kleine Tierfarm");
        _de.Add("AnimalFarmMed","Mittelgroße Tierfarm");
        _de.Add("AnimalFarmLarge","Große Tierfarm");
        _de.Add("AnimalFarmXLarge","Extragroße Tierfarm");
            //Fields
        _de.Add("FieldFarmSmall","Kleine Feldfarm");
        _de.Add("FieldFarmMed","Mittelgroße Feldfarm");
        _de.Add("FieldFarmLarge","Große Feldfarm");
        _de.Add("FieldFarmXLarge","Extragroße Feldfarm");
        _de.Add("FishingHut","Fischerhütte");

            //Raw
        _de.Add("Mortar","Mörtel");
        _de.Add("Clay","Lehm");
        _de.Add("Pottery","Keramik");
        _de.Add("MountainMine","Bergarbeitermine");
        _de.Add("LumberMill" ,"Sägewerk");
        _de.Add("BlackSmith","Schmiede");
        _de.Add("ShoreMine","Ufermine");
        _de.Add("QuickLime","Ungelöschter Kalk");

            //Prod
        _de.Add("Brick","Ziegel");
        _de.Add("Carpentry","Zimmerei");
        _de.Add("Cigars","Zigarren");
        _de.Add("Mill","Mühle");
        _de.Add("Tailor","Schneider");
        _de.Add("Tilery","Fliesenleger");
        _de.Add("Armory","Waffenkammer");
        _de.Add("Distillery","Destillerie");
        _de.Add("Chocolate","Schokolade");
        _de.Add("Ink","Tinte");

            //Ind
        _de.Add("Cloth","Kleidung");
        _de.Add("GunPowder","Schießpulver");
        _de.Add("PaperMill","Papiermühle");
        _de.Add("Printer","Druckerei");
        _de.Add("CoinStamp","Münzprägerei");
        _de.Add("SugarMill","Zuckermühle");
        _de.Add("Foundry","Gießerei");
        _de.Add("SteelFoundry","Stahlgießerei");
        _de.Add("SugarShop","Süßigkeitengeschäft");


            //trade
        _de.Add("Dock","Dock");
        _de.Add("Shipyard","Werft");
        _de.Add("Supplier","Lieferant");
        _de.Add("StorageSmall","Kleines Lager");
        _de.Add("StorageMed","Mittelgroßes Lager");
        _de.Add("StorageBig","Großes Lager");

            //gov
        _de.Add("Library","Bücherei");
        _de.Add("School","Schule");
        _de.Add("TradesSchool","Handelsschule");
        _de.Add("TownHouse","Stadthaus");

            //other
        _de.Add("Church","Kirche");
        _de.Add("Tavern","Taverne");

            //Militar
        _de.Add("WoodPost", "Holzwachturm");
        _de.Add("PostGuard","Steinwachturm");
        _de.Add("Fort","Fort");
        _de.Add("Morro", "Spanische Festung");

            //Decorations
        _de.Add("Fountain", "Springrunnen");
        _de.Add("WideFountain", "Großer Springbrunnen");
        _de.Add("PalmTree", "Palme");
        _de.Add("FloorFountain", "Brunnen");
        _de.Add("FlowerPot", "Blumenkübel");
        _de.Add("PradoLion", "Prado Löwe");

            //Main GUI
        _de.Add("SaveGame.Dialog", "Speichere deinen Spielfortschritt");
        _de.Add("LoadGame.Dialog", "Spiel laden");
        _de.Add("NameToSave", "Speichere dein Spiel unter:");
        _de.Add("NameToLoad", "Spiel zum Laden ausgewählt:");
        _de.Add("OverWrite", "Es gibt bereits ein Spiel mit diesem Namen. Möchtest du die Datei überschreiben?");
        _de.Add("DeleteDialog", "Möchtest du das gespeicherte Spiel wirklich löschen??");
        _de.Add("NotHDDSpace", "Auf dem Laufwerk {0} ist nicht genügend Speicherplatz vorhanden, um das Spiel zu speichern");
        _de.Add("GameOverPirate", "Das war´s leider, du hast das Spiel verloren! Piraten haben deine Stadt angegriffen und alle getötet.");
        _de.Add("GameOverMoney", "Tja aber sorry, du hast das Spiel verloren! Die Krone wird deine karibische Insel nicht mehr unterstützen.");
        _de.Add("BuyRegion.WithMoney", "Möchtest du diese Region wirklich kaufen?.");
        _de.Add("BuyRegion.WithOutMoney", "Sorry, das kannst du dir jetzt nicht leisten.");
        _de.Add("Feedback", "Feedback!? Super ... :) Danke. 8) ");
        _de.Add("OptionalFeedback", "Feedback ist mir sehr wichtig. Bitte lass ein paar Worte da.");
        _de.Add("MandatoryFeedback", "Das sieht nur das Entwicklerteam. Deine Bewertung ist?");
        _de.Add("PathToSeaExplain", "Zeigt den kürzesten Weg zum Meer.");


        _de.Add("BugReport", "Einen Fehler gefunden? ähm, hoppla... schick es auf diesem Weg !! Vielen Dank");
        _de.Add("Invitation", "Die E-Mail-Adresse deines Freundes, um an der Private Beta teilzunehmen");
        _de.Add("Info", "");//use for informational Dialogs
        _de.Add("Negative", "Die Krone hat dir einen Kreditrahmen gewährt. Wenn du mehr als $ 100.000,00 besitzt, ist das Spiel vorbei");  


            //MainMenu
            _de.Add("Types_Explain", "Traditionell: \nDas ist eine Spielvariante, bei dem am Anfang einige Gebäude gesperrt sind und man sie freischalten muss. " +
                    "Das Gute daran ist, dass du hier eine Anleitung bekommst." +
                    "\n\nFreier Wille: \nAlle verfügbaren Gebäude werden sofort freigeschaltet. " +
                    "Das Schlimme daran ist, dass du so leicht versagen kannst." +
                    "\n\nDie Stufe 'Schwer' ist verflucht nah der Realität");


            //Tooltips
            //Small Tooltips
        _de.Add("Person.HoverSmall", "Gesamt/Erwachsene/Kinder");
        _de.Add("Emigrate.HoverSmall", "Auswanderer");
        _de.Add("CurrSpeed.HoverSmall", "Spieltempo");
        _de.Add("Town.HoverSmall", "Stadtname");
        _de.Add("Lazy.HoverSmall", "Arbeitslose Leute");
        _de.Add("Food.HoverSmall", "Nahrung");
        _de.Add("Happy.HoverSmall", "Zufriedenheit");
        _de.Add("PortReputation.HoverSmall", "Ruf des Hafens");
        _de.Add("Dollars.HoverSmall", "Dollar");
        _de.Add("PirateThreat.HoverSmall", "Piratenbedrohung");
        _de.Add("Date.HoverSmall", "Datum (Mmm/J)");
        _de.Add("MoreSpeed.HoverSmall", "Schneller [BildHoch]");
        _de.Add("LessSpeed.HoverSmall", "Langsamer [BildRunter]");
        _de.Add("PauseSpeed.HoverSmall", "Spiel pausieren");
        _de.Add("CurrSpeedBack.HoverSmall", "Aktuelle Geschwindigkeit");
        _de.Add("ShowNoti.HoverSmall", "Benachrichtigungen");
        _de.Add("Menu.HoverSmall", "Hauptmenü");
        _de.Add("QuickSave.HoverSmall", "Schnellspeichern[Strg+S][F]");
        _de.Add("Bug Report.HoverSmall", "Melde einen Fehler");
        _de.Add("Feedback.HoverSmall", "Feedback");
        _de.Add("Hide.HoverSmall", "Verbergen");
        _de.Add("CleanAll.HoverSmall", "Sauber");
        _de.Add("Bulletin.HoverSmall", "Kontrolle/Bulletin");
        _de.Add("ShowAgainTuto.HoverSmall","Tutorial");
        _de.Add("BuyRegion.HoverSmall", "Kaufe Regionen");
        _de.Add("Help.HoverSmall", "Hilfe");

        _de.Add("More.HoverSmall", "Mehr");
        _de.Add("Less.HoverSmall", "Weniger");
        _de.Add("Prev.HoverSmall", "Vorheriges");

        _de.Add("More Positions.HoverSmall", "Mehr");
        _de.Add("Less Positions.HoverSmall", "Weniger");


            //down bar
        _de.Add("Infrastructure.HoverSmall", "Infrastruktur");
        _de.Add("House.HoverSmall", "Häuser");
        _de.Add("Farming.HoverSmall", "Landwirtschaft");
        _de.Add("Raw.HoverSmall", "Güter");
        _de.Add("Prod.HoverSmall", "Produktion");
        _de.Add("Ind.HoverSmall", "Industrie");
        _de.Add("Trade.HoverSmall", "Handel");
        _de.Add("Gov.HoverSmall", "Regierung");
        _de.Add("Other.HoverSmall", "Andere");
        _de.Add("Militar.HoverSmall", "Militär");
        _de.Add("Decoration.HoverSmall", "Dekoration");

        _de.Add("WhereIsTown.HoverSmall", "Zurück zur Stadt [P]");
        _de.Add("WhereIsSea.HoverSmall", "Weg zum Meer anzeigen");
        _de.Add("Helper.HoverSmall", "Hilfe");
        _de.Add("Tempeture.HoverSmall", "Temperatur");
            
            //building window
        _de.Add("Gen_Btn.HoverSmall", "Allgemeiner Tab");
        _de.Add("Inv_Btn.HoverSmall", "Inventar Tab");
        _de.Add("Upg_Btn.HoverSmall", "Verbesserungen Tab");
        _de.Add("Prd_Btn.HoverSmall", "Produktion Tab");
        _de.Add("Sta_Btn.HoverSmall", "Statistiken Tab");
        _de.Add("Ord_Btn.HoverSmall", "Befehle Tab");
        _de.Add("Stop_Production.HoverSmall", "Produktion stoppen");
        _de.Add("Demolish_Btn.HoverSmall", "Abreißen");
        _de.Add("More Salary.HoverSmall", "Zahle mehr");
        _de.Add("Less Salary.HoverSmall", "Zahle weniger");
        _de.Add("Next_Stage_Btn.HoverSmall", "Nächste Stufe kaufen");
        _de.Add("Current_Salary.HoverSmall", "Aktuelles Gehalt");
        _de.Add("Current_Positions.HoverSmall", "Aktuelle Positionen");
        _de.Add("Max_Positions.HoverSmall", "Maximale Positionen");


        _de.Add("Add_Import_Btn.HoverSmall", "Import hinzufügen");
        _de.Add("Add_Export_Btn.HoverSmall", "Export hinzufügen");
        _de.Add("Upg_Cap_Btn.HoverSmall", "Verbesserungskapazität");
        _de.Add("Close_Btn.HoverSmall", "Schließen");
        _de.Add("ShowPath.HoverSmall", "Zeige Pfad");
        _de.Add("ShowLocation.HoverSmall", "Standort anzeigen");//TownTitle
        _de.Add("TownTitle.HoverSmall", "Stadt");
        _de.Add("WarMode.HoverSmall", "Kampfmodus");
        _de.Add("BullDozer.HoverSmall", "Bulldozer");
        _de.Add("Rate.HoverSmall", "Bewerten");

            //addOrder windiw
        _de.Add("Amt_Tip.HoverSmall", "Produktmenge");

            //Med Tooltips 
        _de.Add("Build.HoverMed", "Gebäude platzieren: 'Linksklick' \n" +
                                "Gebäude drehen: 'R' Taste \n" +
                                "Abbrechen: 'Rechtsklick'");
            _de.Add("BullDozer.HoverMed", "Gebiet bereinigen: 'Linksklick' \n" +
                "Abbruch: 'Rechtsklick' \nKosten: $10 pro Nutzung ");

            _de.Add("Road.HoverMed", "Start: 'Linksklick' \n" +
                    "Erweitern: 'Maus bewegen' \n" +
                    "Setzen: 'Nochmal Linksklick' \n" +
                "Abbruch: 'Rechtsklick'");

        _de.Add("Current_Salary.HoverMed", "Die Arbeitnehmer gehen dort arbeiten wo das meiste Gehalt wartet." +
                                            " Wenn 2 Arbeitgeber das gleiche Gehalt zahlen nehmen sie den Job der ihrer Wohnung am nächsten ist.");



            //Notifications
        _de.Add("BabyBorn.Noti.Name", "Neugeborenes");
        _de.Add("BabyBorn.Noti.Desc", "{0} ist geboren");
        _de.Add("PirateUp.Noti.Name", "Piraten nähern sich");
        _de.Add("PirateUp.Noti.Desc", "Piraten sind nahe dem Ufer");
        _de.Add("PirateDown.Noti.Name", "Piraten respektieren dich");
        _de.Add("PirateDown.Noti.Desc", "Piraten respektieren dich für heute ein wenig mehr");

        _de.Add("Emigrate.Noti.Name", "Ein Bürger ist ausgewandert");
        _de.Add("Emigrate.Noti.Desc", "Menschen wandern aus, wenn sie mit deiner Regierung nicht zufrieden sind");
        _de.Add("PortUp.Noti.Name", "Hafen ist bekannt");
        _de.Add("PortUp.Noti.Desc", "Der Ruf deines Hafens steigt mit den benachbarten Häfen und Routen");
        _de.Add("PortDown.Noti.Name", "Hafen ist weniger bekannt");
        _de.Add("PortDown.Noti.Desc", "Der Ruf deines Hafens ist gesunken");

        _de.Add("BoughtLand.Noti.Name", "Neues Land gekauft");
        _de.Add("BoughtLand.Noti.Desc", "Eine neue Landregion wurde gekauft");

        _de.Add("ShipPayed.Noti.Name", "Schiff bezahlt");
        _de.Add("ShipPayed.Noti.Desc", "Ein Schiff hat {0} für Waren oder Dienstleistungen bezahlt");
        _de.Add("ShipArrived.Noti.Name", "Ein Schiff ist angekommen");
        _de.Add("ShipArrived.Noti.Desc", "Ein neues Schiff ist in einem unserer Hafengebäude eingetroffen");

        _de.Add("AgeMajor.Noti.Name", "Neuer Arbeiter");
        _de.Add("AgeMajor.Noti.Desc", "{0} ist bereit für die Arbeit");


        _de.Add("PersonDie.Noti.Name", "Jemand ist verstorben");
        _de.Add("PersonDie.Noti.Desc", "{0} ist verstorben");

        _de.Add("DieReplacementFound.Noti.Name", "Jemand ist verstorben");
        _de.Add("DieReplacementFound.Noti.Desc", "{0} ist verstorben. Es wurde ein Ersatz für den Job gefunden.");

        _de.Add("DieReplacementNotFound.Noti.Name", "Jemand ist verstorben");
        _de.Add("DieReplacementNotFound.Noti.Desc", "{0} ist verstorben. Es wurde kein Ersatz für den Job gefunden");


        _de.Add("FullStore.Noti.Name", "Ein Lager wird voll");
        _de.Add("FullStore.Noti.Desc", "Ein Lager ist bei einer Kapazität von {0}%");

        _de.Add("CantProduceBzFullStore.Noti.Name", "Ein Arbeiter kann nichts herstellen");
        _de.Add("CantProduceBzFullStore.Noti.Desc", "{0} weil sein/ihr Ziellager voll ist");

        _de.Add("NoInput.Noti.Name", "Zumindest fehlt eine Zufuhr zum Gebäude");
        _de.Add("NoInput.Noti.Desc", "Ein Gebäude kann keine {0} erzeugen, da mindestens ein Rohstoff fehlt");

        _de.Add("Built.Noti.Name", "Ein Gebäude wurde fertiggestellt");
        _de.Add("Built.Noti.Desc", "{0} wurde fertiggestellt");

        _de.Add("cannot produce", "kann nicht herstellen");

            



            //Main notificaion
            //Shows on the middle of the screen
        _de.Add("NotScaledOnFloor", "Das Gebäude liegt entweder zu nahe am Ufer oder zu nah am Berg");
        _de.Add("NotEven", "Der Boden unter dem Gebäude ist nicht eben");
        _de.Add("Colliding", "Gebäude kollidiert mit einem anderen");
        _de.Add("Colliding.BullDozer", "Bulldozer kollidiert mit einem Gebäude. Kann nur im Gelände (Bäume, Felsen) verwendet werden");

        _de.Add("BadWaterHeight", "Das Gebäude ist zu tief oder zu hoch auf dem Wasser");
        _de.Add("LockedRegion", "Du musst diese Region besitzen, um hier bauen zu können");
        _de.Add("HomeLess", "Die Leute in diesem Haus können nirgendwo hin gehen. Bitte baue ein neues Haus" +
                            " um diese Familie zu halten und es erneut versuchen");
        _de.Add("LastFood", "Kann nicht zerstört werden, dies ist das einzige Lager in deinem Dorf");
        _de.Add("LastMasonry", "Kann nicht zerstört werden, dies ist der einzige Maurer in deinem Dorf");
        _de.Add("OnlyOneDemolish", "Du zerstörst bereits ein Gebäude. Versuche es erneut, nachdem der Abbruch abgeschlossen ist");


            //help

        _de.Add("Construction.HoverMed", "Für den Bau eines Gebäudes benötigst du Arbeiter im Maurerbetrieb. "+
                    " Klicke auf den Maurer und dann auf das Symbol "+" auf der Registerkarte 'Allgemein'.");
        _de.Add("Demolition.HoverMed", "Sobald die Vorräte frei sind, wird es abgerissen. Schubkarren transportieren die Reste ab");

        _de.Add("Construction.Help", "Für den Bau eines Gebäudes benötigen Sie Arbeiter im Maurerbetrieb. "+
                    " Klicke auf den Maurer und dann auf das Symbol "+" auf der Registerkarte 'Allgemein'. Stelle sicher, dass du über ausreichende Ressourcen verfügst.");
        _de.Add("Camera.Help", "Kamera: Benutze [W,A,S,D] oder den Cursor zum bewegen. " +
                        "Drücke das Scrollrad deiner Maus und halte es gedrückt, um zu drehen, oder nutze [Q] und [E].");
        _de.Add("Sea Path.Help", "Klicke auf die linke untere Ecke 'Seepfad ein- / ausblenden' " +
                            "Schaltfläche, um den nächstgelegenen Weg zum Meer anzuzeigen.");

        _de.Add("People Range.Help", "Der große blaue Kreis um jedes Gebäude markiert die Reichweite.");

        _de.Add("Pirate Threat.Help", "Piratenbedrohung: So bewusst sind sich die Piraten der Existenz deines Hafens. Diese erhöht sich," +
                                        " wenn du mehr Geld besitzt. Wenn dieser Wert über 90 erreicht wird, verlierst du das Spiel. Du kannst der Bedrohung durch den Bau militärischer Gebäude begegnen.");

        _de.Add("Port Reputation.Help", "Ruf des Hafens: Je mehr Leute deinen Hafen kennen, desto mehr besuchen ihn." +
                                            " Wenn du dies verbessern möchtest, stelle sicher, dass du immer einige Aufträge" +
                                            " im Dock hast.");
        _de.Add("Emigrate.Help", "Auswanderer: Wenn die Menschen einige Jahre unglücklich sind, gehen sie. Das Schlechte" +
                                    " daran ist, dass sie nicht wiederkommen, keine Kinder bekommen oder Kinder haben werden." +
                                    " Das einzig Gute ist, dass sie den 'Ruf des Hafens' erhöhen.");
        _de.Add("Food.Help", "Lebensmittel: Je höher die Vielfalt an Lebensmitteln, die in einem Haushalt verfügbar sind, desto zufriedener "+
                                 " werden sie sein.");

        _de.Add("Weight.Help", "Gewicht: Alle Gewichtsangaben im Spiel sind in kg oder Pfund, abhängig davon, welches Einheitensystem ausgewählt ist." +
                                " Du kannst dies unter 'Optionen' im 'Hauptmenü' ändern.");
        _de.Add("What is Ft3 and M3?.Help", "Die Speicherkapazität wird durch das Volumen des Gebäudes bestimmt. Ft3 ist ein Kubikfuß. M3 ist ein Kubikmeter.");//. Denke daran, dass weniger kompakte Produkte dein Lager schnell füllen. Die Produktmenge findest du unter  Bulletin/Prod/Spezi");

        _de.Add("More.Help", "Wenn du weitere Hilfe benötigst, empfiehlt es sich, das Tutorial abzuschließen oder einfach deine Frage in den SugarMill-Foren zu posten.");

                //more 
        _de.Add("Products Expiration.Help", "Produktverfall: Wie in der Realität verfällt jedes Produkt. Einige Lebensmittel verfallen früher als andere. Du kannst unter Bulletin/Prod/Verfall sehen welche Produkte abgelaufen sind.");
        _de.Add("Horse Carriages.Help", "Da das Spiel echte Maße hat, können die Menschen nur begrenzt viel tragen. Danach kommen Pferdefuhrwerk zum Einsatz. Sie transportieren viel mehr, wodurch deine Wirtschaft gestärkt wird. Eine Person in ihren besten Jahren könnte etwa 15 kg, eine Schubkarre ca. 60 kg und der kleinere Wagen kann 240 kg tragen. Um diese zu benutzen, baue eine Schwertransportstation.");
        _de.Add("Usage of goods.Help", "Verwendung von Gütern: Kisten, Fässer, Schubkarren, Karren, Werkzeuge, Kleidung, Geschirr, Möbel und Utensilien sind erforderlich, um die traditionellen Aktivitäten einer Stadt zu erledigen. Wenn diese Waren verwendet werden, verringert sich ihre Anzahl, so dass eine Person nichts trägt, wenn keine Kisten vorhanden sind. Behalte das im Auge ;)");
        _de.Add("Happiness.Help", "Zufriedenheit: Die Zufriedenheit der Menschen wird von verschiedenen Faktoren beeinflusst. Wie viel Geld sie haben, der Lebensmittelvielfalt, Religionszufriedenheit, Zugang zu Freizeit, Hauskomfort und Bildungsniveau. Auch wenn eine Person Zugang zu Utensilien hat, beeinflussen eher Geschirr und Kleidung ihre Zufriedenheit.");
        _de.Add("Line production.Help", "Serienfertigung: Um einen einfachen Nagel herzustellen, muss man Erz abbauen, in der Gießerei das Eisen schmelzen und schließlich muss die Schmiede den Nagel herstellen. Wenn du genug Geld hast, kannst du den Nagel immer direkt auf einem Schiff kaufen wie auch jedes andere Produkt.");
        _de.Add("Bulletin.Help", "Das Seitensymbol in der unteren Leiste ist das Bulletin / Kontrollfenster. Bitte nimm dir etwas Zeit, um es zu erkunden.");
        _de.Add("Trading.Help", "Du benötigst mindestens ein Dock, um handeln zu können. Dort kannst du Import- / Exportaufträge hinzufügen und Bargeld verdienen. Wenn du beim Hinzufügen eines Auftrages Hilfe benötigst, kannst du das Tutorial abschließen.");

        _de.Add("Combat Mode.Help", "Wird aktiviert, wenn ein Pirat/ Bandit von einem deiner Bürger entdeckt wird. Sobald der Modus aktiv ist, kannst du Einheiten direkt den Angriff zuweisen. Wähle sie aus und klicke mit der rechten Maustaste, um das Ziel anzugreifen.");

        _de.Add("Population.Help", "Sobald sie 16 Jahre alt sind, ziehen sie in ein freies Haus, wenn eines zur Verfügung steht. Immer wenn ein freies Haus zur Verfügung steht, wird das Bevölkerungswachstum garantiert. Wenn sie mit 16 Jahren in die neuen Häuser kommen, maximieren Sie das Bevölkerungswachstum.");

        _de.Add("F1.Help", "Drücke [F1] für die Hilfe.");

        _de.Add("Inputs.Help", "Wenn ein Gebäude nichts produzieren kann, weil die Waren fehlen, prüfe bitte ob die erforderlichen Güter im Hauptlager vorhanden sind und mindestens eine Arbeiter im Maurerbetrieb ist.");
        _de.Add("WheelBarrows.Help", "Schubkarren sind die 'Arbeitstiere' der Maurer. Wenn sie nichts zu bauen haben, fungieren sie als Schubkarren. Wenn du Materialien benötigst, die in ein bestimmtes Gebäude gelangen müssen, stelle sicher, dass du über genügend Einheiten verfügst und auch die im Lagergebäude genannten Materialien.");

        _de.Add("Production Tab.Help", "Wenn es sich bei dem Gebäude um ein landwirtschaftliches Feld handelt, stelle sicher, dass sich auf dem Hof Arbeiter befinden. Die Ernte geht verloren, wenn sie einen Monat nach dem Erntetag noch immer dort lagert.");
        _de.Add("Our Inventories.Help", "TDer Abschnitt 'Unsere Lagerbestände' im Fenster 'Auftrag hinzufügen' ist eine Zusammenfassung dessen, was wir aus den Lagerbeständen unserer Lagergebäude erhalten haben.");
        _de.Add("Inventories Explanation.Help", "Dies ist eine Zusammenfassung dessen, was wir in unseren Lagerbeständen haben. Gegenstände in anderen Gebäudebeständen gehören nicht zur Stadt.");

            ///word and grammarly below




            //to  add on spanish         //to correct  
        _de.Add("TutoOver", "Deine Belohnung beträgt $ 10.000,00, wenn du es zum ersten Mal abschließt. Das Tutorial ist nun beendet. Jetzt kannst dieses Spiel entweder weiterspielen oder ein neues Spiel beginnen.");

            //Tuto
        _de.Add("CamMov.Tuto", "Die Belohnung für den Abschluss des Tutorials beträgt $10.000, - (einmalige Belohnung pro Spiel). Schritt 1: Bewege die Kamera mit den Tasten [WASD] oder den Pfeiltasten. Tue dies mindestens 5 Sekunden lang");
        _de.Add("CamMov5x.Tuto", "Verwende die [WASD] oder die Pfeiltasten und halte die linke Umschalttaste gedrückt, um die Kamera fünfmal schneller zu bewegen. Tue dies mindestens 5 Sekunden lang");
        _de.Add("CamRot.Tuto", "Drücke nun das Mausrad nach unten und bewege die Maus, um die Kamera zu drehen. Tue dies mindestens 5 Sekunden lang");


        _de.Add("BackToTown.Tuto", "PDrücke die Taste [P] auf der Tastatur, um zur Ausgangsposition der Kamera zu gelangen");

        _de.Add("BuyRegion.Tuto", "Regionen musst du besitzen, um bauen zu können. Klicke auf das "+" - Zeichen in der unteren Leiste und dann auf das 'Zum Verkauf' -Symbol in der" +
                    " Mitte einer Region, um es zu kaufen. Einige Gebäude sind ausgenommen, sie können gebaut werden, ohne die Region zu besitzen" +
                    " (Fischerhütte, Dock, Bergwerk, Uferbergwerk, Leuchtturm, Wachposten)"
                    );

        _de.Add("Trade.Tuto", "Das war echt einfach, aber der schwierige Teil kommt jetzt. Klicke auf die Schaltfläche 'Handelsgebäude' in der rechten unteren Leiste. "+
                "Wenn du den Mauszeiger darüber bewegst, erscheint ein Popup-Fenster namens 'Handel'.");
        _de.Add("CamHeaven.Tuto", "Scrolle mit der mittleren Maustaste zurück, bis die Kamera den"
                    + " Himmel erreicht. Diese Ansicht ist nützlich, um größere Gebäude wie den Hafen zu platzieren");

        _de.Add("Dock.Tuto", "Klicke nun auf das 'Dock'-Gebäude, es ist der 1. Button. Wenn du darüber schwebbleibst, zeigt es"+
                "die Kosten und die Beschreibung");
        _de.Add("Dock.Placed.Tuto", "Nun zum schweren Teil, daher sorgfältig lesen. Beachte, dass du die "+
                "'R' Taste zum rotieren nutzen kannst und den Rechtklick zum Bauabbruch. Dieses Gebäude hat einen Teil im Ozean und einen anderen an Land." +
                " Der Pfeil zeigt zum Meer, der Lagerbereich ist an Land. Wenn der Pfeil weiß ist, klicke mit der linken Maustaste.");
        _de.Add("2XSpeed.Tuto", "Erhöhe die Geschwindigkeit des Spiels, gehe zum mittleren Geschwindigkeitsregler für die Simulation auf dem oberen Bildschirm und klicke auf die"
                    +" 'Schneller' Schaltfläche 1 Mal bis 2x angezeigt wird");

        _de.Add("ShowWorkersControl.Tuto", "Klicke auf die Schaltfläche 'Steuerung/ Bulletin' in der unteren Leiste. "+
                "Wenn du den Mauszeiger darüber bewegst, wird 'Steuerung / Bulletin' angezeigt.. ");
        _de.Add("AddWorkers.Tuto", "Klicke auf das "+" - Zeichen rechts neben dem Maurerbetrieb. So fügst du weitere Arbeiter hinzu.");
        _de.Add("HideBulletin.Tuto", "Denke bitte daran, dass du in diesem Fenster verschiedene Aspekte des Spiels steuern und sehen kannst. Klicke außerhalb des Fensters, um es zu schließen, oder auf die Schaltfläche 'OK'.");
        _de.Add("FinishDock.Tuto", "Beende nun den Dockbau. Je mehr Arbeiter im Maurerbetrieb sind, desto schneller wird die Arbeit auch erledigt."
            + " Vergewissere dich auch, dass du über alle Materialien verfügst, die für den Bau erforderlich sind");
        _de.Add("ShowHelp.Tuto", "Klicke auf die Schaltfläche 'Hilfe' in der unteren Leiste. "+
                "Wenn du den Mauszeiger darüber bewegst, wird die Hilfe angezeigt. Dort findest du einige hilfreiche Tipps.");


        _de.Add("SelectDock.Tuto", "Schiffe legen Waren nach dem Zufallsprinzip aus dem Inventar des Docks ab. Arbeiter werden benötigt, um Güter auf dem Dock ein- und auszulagern. Sie benötigen Schubkarren und Kisten. Wenn sich keine dieser Materialien im Docklager befinden, arbeiten sie nicht. Klicke nun auf das Dock.");


        _de.Add("OrderTab.Tuto", "Wechsele zur Registerkarte 'Bestellungen' im Dockfenster.");
        _de.Add("ImportOrder.Tuto", "Klicke auf das '+' neben Importauftrag hinzufügen.");

        _de.Add("AddOrder.Tuto", "Scrolle nun in den Produkten nach unten und wähle Holz aus, und gib als Menge 100 ein. Klicke dann auf die Schaltfläche 'Hinzufügen'.");
        _de.Add("CloseDockWindow.Tuto", "Nun wird die Bestellung hinzugefügt. Ein zufälliges Schiff legt diese Gegenstand in das Dockinventar. Ihre Dockarbeiter werden es dann zum nächstgelegenen Lagergebäude bringen. Klicke jetzt außerhalb des Fensters, damit es geschlossen wird.");
        _de.Add("Rename.Tuto", "Klicke auf eine Person und dann auf die Titelleiste der Person. So kannst du den Namen einer beliebigen Person oder eines Gebäudes im Spiel ändern. Klicke außerhalb, um die Änderung zu speichern");
        _de.Add("RenameBuild.Tuto", "Klicke nun auf ein Gebäude und ändere seinen Namen auf dieselbe Weise. Denke daran, außerhalb zu klicken, damit die Änderung gespeichert wird");

        _de.Add("BullDozer.Tuto", "Klicke nun auf das Bulldozer-Symbol in der unteren Leiste. Dann entferne einen Baum oder einen Stein vom Gelände.");


        _de.Add("Budget.Tuto", "Klicke auf die Schaltfläche 'Steuerung / Bulletin', dann auf das Menü 'Finanzen' und dann auf 'Hauptbuch'. Dies ist das Spielkonto");
        _de.Add("Prod.Tuto", "Klicke auf 'Prod' und dann auf 'Produziert'. Dies zeigt die Produktion des Dorfes der letzten 5 Jahre");
        _de.Add("Spec.Tuto", "Klicke auf das Menü 'Prod' und dann auf 'Spez'. Hier kannst du genau sehen, wie du jedes Produkt im Spiel herstellst. Die notwendigen Eingänge und wo wird produziert. Auch die Import- und Exportpreise");
        _de.Add("Exports.Tuto", "Klicke auf das Menü 'Finanzen' und dann auf 'Exportieren'. Hier siehst eine Aufschlüsselung der Exporte deines Dorfes");


                //Quest
        _de.Add("Tutorial.Quest", "Quest: Beende das Tutorial. Belohnung 10.000 $. Dies dauert ungefähr 3 Minuten");

        _de.Add("Lamp.Quest", "Quest: Baue eine Straßenlaterne. Du findest sie in der Infrastruktur. Sie leuchtet nachts, wenn Walöl im Lager ist");

        _de.Add("Shack.Quest", "Quest: Baue eine Hütte. Das sind die billigsten Häuser. Wenn Menschen 16 Jahre alt werden, ziehen sie in ein freies Haus, wenn sie eines finden. Auf diese Weise wird das Bevölkerungswachstum garantiert. [F1] Hilfe. Wenn du Rauch im Schornstein siehst, bedeutet das, dass Menschen darin leben");

        _de.Add("SmallFarm.Quest", "Quest: Baue eine kleine Feldfarm. Du benötigst Farmen, um deine Leute zu ernähren");
        _de.Add("FarmHire.Quest", "Quest: Stellen Sie zwei Bauern auf der kleinen Feldfarm ein. Klicke auf die Farm und klicke das Pluszeichen um Arbeiter einzustellen. Arbeitlos sollten sie aber schon sein"
                    +" um sie einem Gebäude zuweisen zu können");



        _de.Add("FarmProduce.Quest", "Quest: Produziere " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " Bohnen auf einer kleinen Feldfarm. Klicke auf die Registerkarte 'Stat' und zeige die Produktion der letzten 5 Jahre an. Du kannst den Questfortschritt im Questfenster sehen. Wenn du weitere kleine Farmen baust, werden diese für die Quest berücksichtigt");
        _de.Add("Transport.Quest", "Quest: Transportiere nun die Bohnen vom Feld zum Lager. Um dies tun zu können" +
                " solltest du genügend Arbeiter frei haben. Sie fungieren als Transporteure, wenn sie nichts bauen");


        _de.Add("HireDocker.Quest", "Quest: Einen Hafenarbeiter anstellen. Die Aufgabe von Hafenarbeitern besteht nur darin, die Waren aus dem Lager in das Dock zu verschieben, wenn sie exportiert werden sollen."+
            " Natürlich auch umgekehrt beim Import. Sie arbeiten, wenn eine Bestellung vorliegt und die Ware transportbereit ist. Ansonsten bleiben sie zu Hause und ruhen sich aus." +
                " Wie du hast noch kein Dock gebaut? Dann wird´s aber Zeit ein zu bauen."+
            " Du findest es unter Handel." );


        _de.Add("Export.Quest", "Quest: Erstelle im Dock einen Auftrag und exportiere genau 300 " + Unit.CurrentWeightUnitsString() + " Bohnen."+
                " Klicke im Dock auf die Registerkarte 'Bestellungen' und füge einen Exportauftrag mit dem "+" - Zeichen hinzu."+
            " Produkt auswählen und Betrag eingeben");



        _de.Add("MakeBucks.Quest", "Quest: Verdiene $ 100 beim Export von Waren im Dock. "+
            "Sobald ein Schiff ankommt, werden die Produkte nach dem Zufallsprinzip im Inventar Ihres Docks bezahlt");
        _de.Add("HeavyLoad.Quest", "Quest: Baue ein Schwerlastgebäude. Dies sind Spediteure, die mehr Gewicht tragen können. Sie werden nützlich sein, wenn der Transport von Gütern erforderlich ist." ); //In den Lagern der Städte müssen Karren vorhanden sein, damit sie arbeiten können");
        _de.Add("HireHeavy.Quest", "Quest: Im Schwerlastgebäude einen Schwerlasttransporteur einstellen.");


        _de.Add("ImportOil.Quest", "Quest: Importiere 500 " + Unit.CurrentWeightUnitsString() + " Walöl über das Dock. Dies ist erforderlich, um Lichter nachts eingeschaltet zu lassen. Schiffe werden Importe zufällig im Inventar Ihres Dock ablegen");

        _de.Add("Population50.Quest", "Erreiche eine Gesamtbevölkerung von 50 Einwohnern");

            //added Aug 11 2017, result: sep 9(30% off biggest sale ever)
        _de.Add("Production.Quest", "Lass uns jetzt ein paar Waffen herstellen und später verkaufen. Baue dazu zuerst einen Schmied. Diesen findest du im Gebäudemenü unter 'Güter'");
        _de.Add("ChangeProductToWeapon.Quest", "Im 'Produkte Reiter' des Schmieds änderst du die Produktion in Waffen. Die Arbeiter bringen das Rohmaterial mit, wenn es vorrätig ist, um Waffen zu schmieden");
        _de.Add("BlackSmithHire.Quest", "Stelle zwei Schmiedegesellen ein");
        _de.Add("WeaponsProduce.Quest", "Nun produziere " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " Waffen in der Schmiede. Klicke auf die Registerkarte 'Stat' und zeige die Produktion der letzten 5 Jahre an. Du kannst den Questfortschritt im Questfenster verfolgen.");
        _de.Add("ExportWeapons.Quest", "Nun exportiere 100 " + Unit.CurrentWeightUnitsString() + " Waffen. Füge im Dock eine Bestellung für den Export hinzu. Denke stets daran dass Waffen ein sehr profitables Geschäft sind");


        _de.Add("CompleteQuest", "Deine Belohnung ist {0}");


            //added Sep 14 2017
        _de.Add("BuildFishingHut.Quest", "Baue eine Fischerhütte. Auf diese Weise haben die Bürger verschiedene Nahrungsmittel zur Verfügung, was sich in Zufriedenheit niederschlägt");
        _de.Add("HireFisher.Quest", "Stelle einen Fischer ein");

        _de.Add("BuildLumber.Quest", "Baue eine Holzfällermühle. Du findest sie im Gebäude-Menü unter 'Güter'");
        _de.Add("HireLumberJack.Quest", "Stelle einen Holzfäller ein");

        _de.Add("BuildGunPowder.Quest", "Baue eine Schießpulverfabrik. Du findest sie im Gebäude-Menü unter 'Industrie'");
        _de.Add("ImportSulfur.Quest", "Importiere bei den Docks 1000 " + Unit.CurrentWeightUnitsString() + " Schwefel");
        _de.Add("GunPowderHire.Quest", "Stelle einen Arbeiter für die Schießpulverfabrik ein");

        _de.Add("ImportPotassium.Quest", "Importiere bei den Docks 1000" + Unit.CurrentWeightUnitsString() + " Kalium");
        _de.Add("ImportCoal.Quest", "Importiere bei den Docks 1000" + Unit.CurrentWeightUnitsString() + " K");

        _de.Add("ProduceGunPowder.Quest", "Produziere nun" + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " Schießpulver. Beachte, dass du Schwefel, Kalium und Kohle benötigst, um Schießpulver herzustellen");
        _de.Add("ExportGunPowder.Quest", "Exportiere bei den Docks 100 " + Unit.CurrentWeightUnitsString() + " Schießpulver");

        _de.Add("BuildLargeShack.Quest", "Baue eine große Hütte. In diesen größeren Häusern  wird die Bevölkerung schneller wachsen");

        _de.Add("BuildA2ndDock.Quest", "Baue ein zweites Dock. Dieses Dock kann nur für Importe verwendet werden. Auf diese Weise kannst du Rohstoffe hier importieren und an einem anderen Dock exportieren");
        _de.Add("Rename2ndDock.Quest", "Benenne die Docks jetzt um, so dass du leichter erkennst, welche nur für Importe und welche für Exporte verwendet werden");

        _de.Add("Import2000Wood.Quest", "Importiere nun auf dem Importdock 2000 " + Unit.CurrentWeightUnitsString() + " Holz. Dieses Rohmaterial wird für alles mögliche benötigt. Allerdings ist es auch als Brennstoff");

            //IT HAS FINAL MESSAGE 
            //last quest it has a final message to the player. if new quest added please put the final message in the last quest
        _de.Add("Import2000Coal.Quest", "Imortiere nun im Importdock 2000 " + Unit.CurrentWeightUnitsString() + " Kohle. Kohle wird auch für alles benötigt, weil sie als Brennstoff verwendet wird. Ich hoffe, du genießt die bisherige Spielerfahrung. Erweitere deine Kolonie und deinen Wohlstand. Bitte hilf auch mit das Spiel zu verbessern. Beteilige dich an unserem Online-Forum, denn deine Stimme und deine Meinung sind wichtig! Viel Spaß Sugarmiller!");

            //



            //Quest Titles
        _de.Add("Tutorial.Quest.Title", "Tutorial");
        _de.Add("Lamp.Quest.Title", "Straßenlaterne");

        _de.Add("Shack.Quest.Title", "Baue eine Hütte");
        _de.Add("SmallFarm.Quest.Title", "Baue ein Farmfeld");
        _de.Add("FarmHire.Quest.Title", "Stelle zwei Farmer ein");


        _de.Add("FarmProduce.Quest.Title", "Landwirtschaftlicher Produzent");

        _de.Add("Export.Quest.Title", "Exporte");
        _de.Add("HireDocker.Quest.Title", "Stelle einen Dockarbeiter ein");
        _de.Add("MakeBucks.Quest.Title", "Verdiene Geld");
        _de.Add("HeavyLoad.Quest.Title", "Schwerlasttransport");
        _de.Add("HireHeavy.Quest.Title", "Stelle einen Schwerlasttransporteur ein");

        _de.Add("ImportOil.Quest.Title", "Walöl");

        _de.Add("Population50.Quest.Title", "50 Einwohner");
            
            //
        _de.Add("Production.Quest.Title", "Stelle Waffen her");
        _de.Add("ChangeProductToWeapon.Quest.Title", "Ändere ein Produkt");
        _de.Add("BlackSmithHire.Quest.Title", "Stelle zwei Schmiedegesellen ein");
        _de.Add("WeaponsProduce.Quest.Title", "Schmiede Waffen");
        _de.Add("ExportWeapons.Quest.Title", "Mach ordentlich Profit" );
            
            //
        _de.Add("BuildFishingHut.Quest.Title", "Baue eine Fischerhütte");
        _de.Add("HireFisher.Quest.Title", "Stelle einen Fischer ein");
        _de.Add("BuildLumber.Quest.Title", "Baue eine Sägemühle");
        _de.Add("HireLumberJack.Quest.Title", "Stelle einen Holzfäller ein");
        _de.Add("BuildGunPowder.Quest.Title", "Baue eine Schießpulverfabrik");
        _de.Add("ImportSulfur.Quest.Title", "Imortiere Schwefel");
        _de.Add("GunPowderHire.Quest.Title", "Stelle einen Schießpulverfabrikarbeiter ein");
        _de.Add("ImportPotassium.Quest.Title", "Importiere Kalium");
        _de.Add("ImportCoal.Quest.Title", "Importiere Kohle");
        _de.Add("ProduceGunPowder.Quest.Title", "Stelle Schießpulver her");
        _de.Add("ExportGunPowder.Quest.Title", "Exortiere Schießpulver");
        _de.Add("BuildLargeShack.Quest.Title", "Baue eine große Hütte");
        _de.Add("BuildA2ndDock.Quest.Title", "Baue ein zweites Dock");
        _de.Add("Rename2ndDock.Quest.Title", "Benenne das zweite Dock um");
        _de.Add("Import2000Wood.Quest.Title", "Importiere etwas Holz");
        _de.Add("Import2000Coal.Quest.Title", "Importiere etwas Kohle");




        _de.Add("Tutorial.Arrow", "Dies ist das Tutorial. Sobald du es abgeschlossen hast, kannst du 10.000 $ einsacken");
        _de.Add("Quest.Arrow", "Dies ist die Aufgaben-Schaltfläche. Du kannst hier auf das Aufgabenfenster zugreifen, indem du einfach darauf klickst");
        _de.Add("New.Quest.Avail", "Mindestens eine Aufgabe ist verfügbar");
        _de.Add("Quest_Button.HoverSmall", "Aufgabe");



            //Products
            //Notification.Init()
        _de.Add("RandomFoundryOutput", "Geschmolzenes Erz");

            //OrderShow.ShowToSetCurrentProduct()
        _de.Add("RandomFoundryOutput (Ore, Wood)", "Geschmolzenes Erz (Erz, Holz)");



            //Bulleting helps
        _de.Add("Help.Bulletin/Prod/Produce", "Hier wird angezeigt, was im Dorf produziert wird.");
        _de.Add("Help.Bulletin/Prod/Expire", "Hier wird angezeigt, was im Dorf an Waren verfallen ist.");
        _de.Add("Help.Bulletin/Prod/Consume", "Hier wird angezeigt, was von deinen Leuten verbraucht wird.");

        _de.Add("Help.Bulletin/Prod/Spec", "In diesem Fenster kannst du die, für jedes Produkt erforderlichen Materialien sehen, wo es hergestellt wird und den Preis. "
            + "Scrolle nach oben, um die Kopfzeilen anzuzeigen. Beachte dass ein einfaches Produkt mehr als nur ein Grundprodukt sein kann.");

        _de.Add("Help.Bulletin/General/Buildings", "Dies ist eine Zusammenfassung wie viele Gebäude von jedem Typ bereits existieren.");

        _de.Add("Help.Bulletin/General/Workers", "In diesem Fenster können Arbeiter den verschiedenen Gebäuden zugewiesen werden. "
            + "Damit ein Gebäude mehr Menschen Arbeit bieten kann, muss es unter der Kapazität liegen und muss mindestens einen Arbeitslosen finden.");

        _de.Add("Help.Bulletin/Finance/Ledger", "Hier wird das Hauptbuch angezeigt. Das Gehalt ist der Betrag, welcher an einen Arbeitnehmer gezahlt wird. Je mehr Menschen arbeiten, desto mehr wird ausbezahlt.");
        _de.Add("Help.Bulletin/Finance/Exports", "Ein Zusammenbruch der Exporte");
        _de.Add("Help.Bulletin/Finance/Imports", "Ein Zusammenbruch der Importe");


        _de.Add("Help.Bulletin/Finance/Prices", "Hilfe Bulletin/Finanzen/Preise");


        _de.Add("LoadWontFit", "Diese Ladung passt nicht in den Lagerbereich");

        _de.Add("Missing.Input", "Gebäude kann nichts produzieren (Materialien müssen sich zuerst im Gebäudeinventar befinden). Fehlende Materialien: \n" );





            //in game
            
        _de.Add("Buildings.Ready", "\n Gebäude fertig zum Bau:");
        _de.Add("People.Living", "Menschen, die in diesem Haus leben:");
        _de.Add("Occupied:", "Gefüllt:");
        _de.Add("|| Capacity:", "|| Kapazität:");
        _de.Add("Users:", "\nUsers:");
        _de.Add("Amt.Cant.Be.0", "Menge kann nicht 0 sein ");
        _de.Add("Prod.Not.Select", "Bitte wähle ein Produkt");


            //articles
        _de.Add("The.Male", "Der");
        _de.Add("The.Female", "Die");

            //
        _de.Add("Build.Destroy.Soon", "Dieses Gebäude wird bald zerstört. Wenn das Inventar nicht leer ist, muss es mit Schubkarren abtransportiert werden");




            //words
            //Field Farms
        _de.Add("Bean", "Bohnen");
        _de.Add("Potato", "Kartoffeln");
        _de.Add("SugarCane", "Zuckerrohr");
        _de.Add("Corn", "Getreide");
        _de.Add("Cotton", "Baumwolle");
        _de.Add("Banana", "Bananen");
        _de.Add("Coconut", "Kokosnüsse");
            //Animal Farm
        _de.Add("Chicken", "Hühner");
        _de.Add("Egg", "Eier");
        _de.Add("Pork", "Schweine");
        _de.Add("Beef", "Rindfleisch");
        _de.Add("Leather", "Leder");
        _de.Add("Fish", "Fisch");
            //mines
        _de.Add("Gold", "Gold");
        _de.Add("Stone", "Stein");
        _de.Add("Iron", "Eisen");

            // { "Clay", "Lehm");
        _de.Add("Ceramic", "Keramik");
        _de.Add("Wood", "Holz");

            //Prod
        _de.Add("Tool", "Werkzeuge");
        _de.Add("Tonel", "Ton");
        _de.Add("Cigar", "Zigarren");
        _de.Add("Tile", "Ziegel");
        _de.Add("Fabric", "Fabrik");
        _de.Add("Paper", "Papier");
        _de.Add("Map", "Karte");
        _de.Add("Book", "Buch");
        _de.Add("Sugar", "Zucker");
        _de.Add("None", "Nichts");
            //
        _de.Add("Person", "Person");
        _de.Add("Food", "Nahrung");
        _de.Add("Dollar", "Dollar");
        _de.Add("Salt", "Salz");
        _de.Add("Coal", "Kohle");
        _de.Add("Sulfur", "Schwefel");
        _de.Add("Potassium", "Kalium");
        _de.Add("Silver", "Silber");
        _de.Add("Henequen", "Agave");
            //
        _de.Add("Sail", "Segel");
        _de.Add("String", "Seil");
        _de.Add("Nail", "Nägel");
        _de.Add("CannonBall", "Kanonenkugel");
        _de.Add("TobaccoLeaf", "Tabakblatt");
        _de.Add("CoffeeBean", "Kaffebohne");
        _de.Add("Cacao", "Kakao");
        _de.Add("Weapon", "Waffe");
        _de.Add("WheelBarrow", "Schubkarre");
        _de.Add("WhaleOil", "Walöl");
            //
        _de.Add("Diamond", "Diamanten");
        _de.Add("Jewel", "Juwelen");
        _de.Add("Rum", "Rum");
        _de.Add("Wine", "Wein");
        _de.Add("Ore", "Erz");
        _de.Add("Crate", "Kiste");
        _de.Add("Coin", "Münzen");
        _de.Add("CannonPart", "Kanonenteil");
        _de.Add("Steel", "Stahl");
            //
        _de.Add("CornFlower", "Sonnenblume");
        _de.Add("Bread", "Brot");
        _de.Add("Carrot", "Karotte");
        _de.Add("Tomato", "Tomate");
        _de.Add("Cucumber", "Gurke");
        _de.Add("Cabbage", "Kohl");
        _de.Add("Lettuce", "Grüner Salat");
        _de.Add("SweetPotato", "Süßkartoffel");
        _de.Add("Yucca", "Yucca");
        _de.Add("Pineapple", "Ananas");
            //
        _de.Add("Papaya", "Papaya");
        _de.Add("Wool", "Wolle");
        _de.Add("Shoe", "Schuh");
        _de.Add("CigarBox", "Zigarrenkiste");
        _de.Add("Water", "Wasser");
        _de.Add("Beer", "Bier");
        _de.Add("Honey", "Honig");
        _de.Add("Bucket", "Eimer");
        _de.Add("Cart", "Wagen");
        _de.Add("RoofTile", "Dachteil");
        _de.Add("FloorTile", "Bodenteil");
        _de.Add("Furniture", "Möbel");
        _de.Add("Crockery", "Geschirr");

        _de.Add("Utensil", "Utensilien");
        _de.Add("Stop", "Stop");


            //more Main GUI
        _de.Add("Workers distribution", "Arbeiterverteilung");
        _de.Add("Buildings", "Gebäude");

        _de.Add("Age", "Alter");
        _de.Add("Gender", "Geschlecht");
        _de.Add("Height", "Größe");
        _de.Add("Weight", "Gewicht");
        _de.Add("Calories", "Kalorien");
        _de.Add("Nutrition", "Ernährung");
        _de.Add("Profession", "Beruf");
        _de.Add("Spouse", "Ehepartner");
        _de.Add("Happinness", "Zufriedenheit");
        _de.Add("Years Of School", "Schuljahre");
        _de.Add("Age majority reach", "Alter mehrheitlich erreicht");
        _de.Add("Home", "Zuhause");
        _de.Add("Work", "Arbeit");
        _de.Add("Food Source", "Nahrungsquellen");
        _de.Add("Religion", "Religion");
        _de.Add("Chill", "Ausgeruhtheit");
        _de.Add("Thirst", "Durst");
        _de.Add("Account", "Konto");

        _de.Add("Early Access Build", "Early Access Version");

            //Main Menu
        _de.Add("Resume Game", "Spiel fortsetzen");
        _de.Add("Continue Game", "Weiterspielen");
        _de.Add("Tutorial(Beta)", "Tutorial(Beta)");
        _de.Add("New Game", "Neues Spiel");
        _de.Add("Load Game", "Spiel laden");
        _de.Add("Save Game", "Spiel speichern");
        _de.Add("Achievements", "Errungenschaften");
        _de.Add("Options", "Optionen");
        _de.Add("Exit", "Beenden");
            //Screens
            //New Game
        _de.Add("Town Name:", "Stadtname:");
        _de.Add("Difficulty:", "Schwierigkeitsgrad:");
        _de.Add("Easy", "Leicht");
        _de.Add("Moderate", "Moderat");
        _de.Add("Hard", "Schwer");
        _de.Add("Type of game:", "Art des Spiels:");
        _de.Add("Freewill", "Freier Wille");
        _de.Add("Traditional", "Traditionell");
        _de.Add("New.Game.Pirates", "Piraten (wenn geprüft, könnte die Stadt einen Piratenangriff erleiden)");
        _de.Add("New.Game.Expires", "Lebensmittelverfall (wann geprüfte Lebensmittel ablaufen)");
        _de.Add("OK", "OK");
        _de.Add("Cancel", "Abbruch");
        _de.Add("Delete", "Löschen");
        _de.Add("Enter name...", "Namen eingeben...");
            //Options
        _de.Add("General", "Allgemein");
        _de.Add("Unit System:", "Einheitensystem:");
        _de.Add("Metric", "Metrisch");
        _de.Add("Imperial", "Imperial");
        _de.Add("AutoSave Frec:", "Automatisch speichern:");
        _de.Add("20 min", "20 min");
        _de.Add("15 min", "15 min");
        _de.Add("10 min", "10 min");
        _de.Add("5 min", "5 min");
        _de.Add("Language:", "Sprache:");
        _de.Add("English", "Englisch");
        _de.Add("Camera Sensitivity:", "Kamerasensivität:");
        _de.Add("Themes", "Themen");
        _de.Add("Halloween:", "Halloween:");
        _de.Add("Christmas:", "Weihnachten:");
        _de.Add("Options.Change.Theme", "Wenn geändert, lade bitte das Spiel einmal neu");

        _de.Add("Screen", "Bildschirm");
        _de.Add("Quality:", "Qualität:");
        _de.Add("Beautiful", "Schön");
        _de.Add("Fantastic", "Fantastisch");
        _de.Add("Simple", "Einfach");
        _de.Add("Good", "Gut");
        _de.Add("Resolution:", "Auflösung:");
        _de.Add("FullScreen:", "Vollbild:");

        _de.Add("Audio", "Audio");
        _de.Add("Music:", "Musik:");
        _de.Add("Sound:", "Klang:");
        _de.Add("Newborn", "Neugeborenes");
        _de.Add("Build Completed", "Bau abgeschlossen");
        _de.Add("People's Voice", "Volkes Stimme");
            
            //in game gui
        _de.Add("Prod", "Produktion");
        _de.Add("Finance", "Finanzen");


            
            //After Oct 20th 2018
        _de.Add("Resources", "Ressourcen");
        _de.Add("Dollars", "Dollar");
        _de.Add("Coming.Soon", "Dieses Gebäude ist erst später im Spiel verfügbar");
        _de.Add("Max.Population", "Du kannst keine Häuser mehr bauen. Die maximale Population ist erreicht");

        _de.Add("To.Unlock", "Zum freischalten: ");
        _de.Add("People", "Menschen");
        _de.Add("Of.Food", " Nahrung. ");
        _de.Add("Port.Reputation.Least", "Ansehen des Hafens bei mindestens ");
        _de.Add("Pirate.Threat.Less", "Piratenbedrohung niedriger als ");
        _de.Add("Skip", "Überspringen");

            //After Dec 8, 2018
        _de.Add("ReloadMod.HoverSmall", "Lade Mod Dateien neu");
        _de.Add("isAboveHeight.MaritimeBound", "Der Grundstücksbereich des Gebäudes liegt unter der zulässigen Höhe");
        _de.Add("arePointsEven.MaritimeBound", "Der Grundstücksbereich des Gebäudes befindet sich nicht auf ebenen Gelände");
        _de.Add("isOnTheFloor.MaritimeBound", "Der Grundstücksbereich des Gebäudes befindet sich nicht auf der üblichen Höhe");
        _de.Add("isBelowHeight.MaritimeBound", "Der maritime Teil des Gebäudes muss im Wasser liegen");

        _de.Add("InLand.Helper", "an Land");
        _de.Add("InWater.Helper", "auf Wasser");

            //After Dec 28, 2018
        _de.Add("Down.HoverSmall", "Priorität verringern");
        _de.Add("Up.HoverSmall", "Priorität erhöhen");
        _de.Add("Trash.HoverSmall", "Auftrag löschen");
        _de.Add("Counting...", "Zähle...");
        _de.Add("Ten Orders Limit", "Das Limit für Aufträge liegt bei 10");

            //After May 1, 2019
        _de.Add("Our inventories:", "Unsere Bestände:");
        _de.Add("Select Product:", "Produktauswahl:");
        _de.Add("Current_Rank.HoverSmall", "Nummer in der Warteschlange");

        _de.Add("Deutsch(Beta)", "Deutsch(Beta)");
        _de.Add("Deutsch", "Deutsch");



        //
        // 
        // Below It needs to be double checked by Karsten. Dec 20, 2019

        //Dec 14

        //in game gui

        _de.Add("Help", "Hilfe" );
        _de.Add("Quest", "Aufgabe" );
        _de.Add("Add Order", "Auftrag zufügen" );
        _de.Add("Suggest Change", "Vorschläge" );

        _de.Add("Panel Control / Bulletin", "Kontrollfeld" );
        _de.Add("Exports", "Exporte" );
        _de.Add("Ledger", "Hauptbuch" );

        _de.Add("Consume", "Verbrauch" );
        _de.Add("Produce", "Produktion" );
        _de.Add("Expire", "Verfall" );

        _de.Add("Spec", "Spezifikation" );
        _de.Add("Input1", "Eingabe1" );
        _de.Add("Input2", "Eingabe2" );
        _de.Add("Input3", "Eingabe3" );
        _de.Add("Building", "Gebäude" );
        _de.Add("Price", "Preis" );

        _de.Add("Date", "Datum" );
        _de.Add("Product", "Produkt" );
        _de.Add("Amount", "Menge" );
        _de.Add("Transaction", "Transaktion" );

        _de.Add("Workers", "Arbeiter" );

        //Help
        _de.Add("Bulletin", "Bulletin" );
        _de.Add("Construction", "Konstruktion" );
        _de.Add("Happiness", "Zufriedenheit" );
        _de.Add("Horse Carriages", "Pferdekutschen" );
        _de.Add("Inputs", "Billets" );
        _de.Add("Line production", "Serienfertigung" );
        _de.Add("Our Inventories", "Unsere Bestände" );
        _de.Add("Inventories Explanation", "Vorräte Erläuterung" );
        _de.Add("People Range", "Persönliche Reichweite" );
        _de.Add("Pirate Threat", "Piratenbedrohung" );
        _de.Add("Population", "Bevölkerung" );
        _de.Add("Port Reputation", "Ansehen des Hafens" );
        _de.Add("Production Tab", "Produktionsreiter" );
        _de.Add("Products Expiration", "Produktablauf" );
        _de.Add("Sea Path", "Seeweg" );
        _de.Add("Trading", "Handel" );
        _de.Add("Usage of goods", "Verwendung von Waren" );
        _de.Add("What is Ft3 and M3?", "Was ist Ft3 und M3?" );
        _de.Add("WheelBarrows", "Schubkarren" );

          //All Lang Needed for sure
        
        _de.Add("Unemployed", "Arbeitslos" );

        //Budget
        _de.Add("Budget Resumen", "Konten" );
        _de.Add("Initial Balance", "Anfangsguthaben" );
        _de.Add("Income", "Einkommen" );
        _de.Add("Quests Completion", "Abschluss der Aufgabe" );
        _de.Add("Income Subtotal", "Zwischensumme der Einnahmen" );

        _de.Add("Expenses", "Kosten" );
        _de.Add("New bought lands", "Neu gekaufte Grundstücke" );
        _de.Add("Salary", "Gehalt" );
        _de.Add("Expenses Subtotal", "Zwischensumme der Ausgaben" );

        _de.Add("Year", "Jahr" );
        _de.Add("Imports", "Importe" );
        _de.Add("Balance", "Bilanz" );
        _de.Add("Year Balance", "Jahresbilanz" );
        _de.Add("Ending Balance", "Endbilanz" );


        _de.Add("Command Keys", "Befehlstasten");
        _de.Add("Command Keys.Text", "[F1] Hilfe\n[F9] GUI ein- / ausblenden\n[P] Kamera auf das Dorf zentrieren");
        _de.Add("Credits", "Credits");
        _de.Add("Credits.Text", "Übersetzung:\nCédric Gauché (fr)\nKarsten Eidner (de)");
        _de.Add("Loading...", "Lade...");

        //Quest window

        _de.Add("ShowQuest.HoverSmall", "Aufgabe anzeigen");
        _de.Add("Have Fun", "Viel Spaß!");
        _de.Add("Current Quest:", "Aktuelle Quest:");
        _de.Add("Reward: ", "Belohnung: ");
        _de.Add("Reward:", "Belohnung:");

        _de.Add("Status: ", "Status: ");
        _de.Add("Status", "Status");
        _de.Add("Active", "Aktiv");
        _de.Add("Done", "Abgeschlossen");

        _de.Add(" of ", " von ");

        //After May 1, 2019
        _de.Add("Construction.Progress", "Baufortschritt: ");
        _de.Add("Warning.This.Building", "Achtung: Dieses Gebäude kann derzeit nicht gebaut werden. Fehlendes Material(de):\n");
        _de.Add("Product.Selected", "Ausgewähltes Produkt: ");
        _de.Add("Harvest.Date", "\nErntedatum: ");
        _de.Add("Progress", "\nFortschritt: ");

        //AddOrderWindow.cs
        _de.Add("Add.New", "Neu hinzufügen");
        _de.Add("Order", "Bestellung");
        _de.Add("Import", "Import");
        _de.Add("Export", "Export");
        //AddOrderWindow GUI
        _de.Add("Enter Amount:", "Menge eingeben:");
        _de.Add("Enter amount...", "Menge eingeben...");
        _de.Add("New Order:", "Neue Bestellung:");
        _de.Add("Product:", "Produkt:");
        _de.Add("Amount:", "Menge:");
        _de.Add("Order total price:", "Gesamtpreis der Bestellung:");
        _de.Add("Add", "Hinzufügen");

        //BuildingWindow GUI
        _de.Add("Product Description:", "Produktbeschreibung:");
        _de.Add("Production report by years:", "Produktionsbericht nach Jahren:");
        _de.Add("Import Orders", "Importbestellungen");
        _de.Add("Export Orders", "Exportbestellungen");
        _de.Add("Orders in progress:", "Bestellungen in Bearbeitung:");

        _de.Add("Notifications", "Benachrichtigungen");

		_de.Add("Attention.Production", "Achtung: Produktion wurde eingestellt. Um die Produktion in diesem Gebäude wieder aufzunehmen, wähle ein Produkt aus");
		_de.Add("Selected product: ", "Produkt auswählen: ");
		_de.Add("Price: ", "Preis: ");
		_de.Add(" per ", " pro ");
		_de.Add("Inputs needed per ", "Eingaben benötigt von");
		_de.Add("Inventory:", "Inventar:");

		_de.Add("CornMeal", "Maismehl");
		_de.Add("PalmLeaf", "Palmblätter");
		_de.Add("Rubber", "Gummi");

        //Dec 9, 2019
        _de.Add("Barrel", "Fass");
        _de.Add("Years of school", "Schuljahre");
        _de.Add("House comfort", "Hauskomfort");
        _de.Add("Food source", "Weingut");
        _de.Add("Relax", "Entspannung");
        _de.Add("Male", "Männlich");
        _de.Add("Female", "Weiblich");
        _de.Add("Quenched", "Gesättigt");

        _de.Add("Sand", "Sand");
        _de.Add("Machinery", "Maschinen");
        _de.Add("Cassava", "Maniok");
        _de.Add("Candy", "Süssigkeiten");

        _de.Add("Tutorial", "Tutorial");

        //Dec 20 2019 for all Langs
        _de.Add("I.Can.Service", "\n\nIch kann warten ");

        //Dec 28
        _de.Add("Finanzen", "Finanzen");
        _de.Add("Camera", "Kamera");
        _de.Add("More", "Mehr");

        //Mar 5th 2020
        _de.Add("Day Cycle:", "Nacht:");

        //Mar 20 2020
        _de.Add("Rotten", "Verfault");
        _de.Add("Ready", "Bereit");
    }

    internal static void Clear()
    {
        _de.Clear();
    }

    public static string ReturnValueWithKey(string key)
    {
        return _de.ReturnValueWithKey(key);
    }

    public static bool ContainsKey(string key)
    {
        return _de.ContainsKey(key);
    }

}