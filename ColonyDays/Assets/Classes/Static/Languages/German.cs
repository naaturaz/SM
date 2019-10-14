using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class German
{
    private static Dictionary<string, string> _german = new Dictionary<string, string>();

    /// </summary>
    public static void ReloadDict()
    {
        string _houseTail = ". Hiel lebt er nun der SugarMiller und genießt wenigstens ab und zu eine gute Mahlzeit";
        string _animalFarmTail = ", In diesem Gebäude können verschiedene Arten Tiere gezüchtet werden";
        string _fieldFarmTail = ", In diesem Gebäude können verschiedene Pflanzen und Früchte angebaut werden";
        string _asLongHasInput = ", solange die notwendigen Komponenten verfügbar sind";
        string _produce = "In diesem Gebäude wird von den Arbeitern das ausgewählte Produkt hergestellt, sofern die erforderlichen Materialien verfügbar sind";
        string _storage =
        "Dies ist ein Lagergebäude. Wenn es voll wird, arbeiten die Leute nicht mehr, da sie nichts haben, wo sie ihre Produkte lagern können";
        string _militar = "Dieses Gebäude trägt dazu bei, die Piratenbedrohung in Ihrem Hafen zu verringern. Um effektiv zu sein, müssen Arbeiter darin sein. Je mehr Arbeiter desto besser";
        string _notRegionNeeded = " Kann gebaut werden, ohne die Region zu besitzen.";

        _german = new Dictionary<string, string>()
        {
            //Descriptions
            //Infr
            { "Road.Desc","Wird zu Dekorationszwecken verwendet. Die Menschen sind zufriedener, wenn sie Straßen vorfinden"},
            { "BridgeTrail.Desc","Erlaubt es den Leuten, von einer Seite des Geländes zur anderen zu gelangen"},
            { "BridgeRoad.Desc","Erlaubt es den Leuten, von einer Seite des Geländes zur anderen zu gelangen. Die Menschen lieben diese Brücken. Sie geben ihnen ein Gefühl von Wohlstand und Glück" +_houseTail},
            { "LightHouse.Desc","Hilft, die Sichtbarkeit des Hafens zu erhöhen. Fügt dem Hafen einen Ruf hinzu, solange Arbeiter in ihm tätig sind"},
            { H.Masonry + ".Desc","Wichtiges Gebäude, Arbeiter bauen neue Gebäude und arbeiten als Transporteure, wenn sie nichts zu tun haben"},
            { H.StandLamp + ".Desc","Leuchtet nachts, wenn Walöl im Lager der Stadt verfügbar ist"},

            { H.HeavyLoad + ".Desc","Diese Arbeiter benutzen Pferdefuhrwerke, um Waren zu transportieren"},


            //House
            { "Bohio.Desc", "Bohiohaus, primitive Bedingungen mit unzufiedenen Menschen, die maximal 2 bis 3 Kinder haben können"},

            { "Shack.Desc", "Hütte, primitive Verhältnisse mit unzufriedenen Menschen, die maximal 2 Kinder haben können"},
            { "MediumShack.Desc", "Die mittelgroße Hütte, mit primitiven Bedingungen für ein wenig mehr Zufriedenheit, kann maximal 2 bis 3 Kinder haben"},
            { "LargeShack.Desc", "Die große Hütte, hat etwas bessere Bedingungen, die Menschen sind zufriedener und können maximal 2 bis 4 Kinder haben"},


            { "WoodHouseA.Desc", "Mittelgroßes Holzhaus, eine Familie kann maximal 2-3 Kinder haben" },
            { "WoodHouseB.Desc", "Mittelgroßes Holzhaus, eine Familie kann maximal 3-4 Kinder haben"  },
            { "WoodHouseC.Desc", "Mittelgroßes Holzhaus, eine Familie kann maximal 2-3 Kinder haben"},
            { "BrickHouseA.Desc", "Mittleres Haus, eine Familie kann maximal 3 Kinder haben"},
            { "BrickHouseB.Desc","Großes Haus, eine Familie kann maximal 3-4 Kinder haben"},
            { "BrickHouseC.Desc","Großes Haus, eine Familie kann maximal 4 Kinder haben"},

            
            //Farms
            //Animal
            { "AnimalFarmSmall.Desc","Kleine Tierfarm"+_animalFarmTail},
            { "AnimalFarmMed.Desc","Mittelgroße Tierfarm"+_animalFarmTail},
            { "AnimalFarmLarge.Desc","Große Tierfarm"+_animalFarmTail},
            { "AnimalFarmXLarge.Desc","Extragroße Tierfarm"+_animalFarmTail},
            //Fields
            { "FieldFarmSmall.Desc","Kleine Feldfarm"+_fieldFarmTail},
            { "FieldFarmMed.Desc","Mittelgroße Feldfarm"+_fieldFarmTail},
            { "FieldFarmLarge.Desc","Große Feldfarm"+_fieldFarmTail},
            { "FieldFarmXLarge.Desc","Extragroße Feldfarm"+_fieldFarmTail},
            { H.FishingHut + ".Desc","Hier kann ein Arbeiter in einem Fluss Fische fangen (muss an einem Fluss plaziert werden)." + _notRegionNeeded},

            //Raw
            { "Mortar.Desc","Hier wird ein Arbeiter Mörtel produzieren"},
            { "Clay.Desc","Hier wird ein Arbeiter Ton produzieren, das Rohmaterial für Ziegel und mehr"},
            { "Pottery.Desc","Hier produziert ein Arbeiter Keramikprodukte wie Geschirr, Gläser usw."},
            { "Mine.Desc","Hier kann ein Arbeiter in einem Fluss fischen"},
            { "MountainMine.Desc","Hier wird ein Arbeiter in der Mine Erz fördern"},
            { "Resin.Desc","Hier wird ein Arbeiter in der Mine arbeiten, und zufällige Mineralien und Metalle gewinnen."},
            {  H.LumberMill +".Desc","Hier finden Arbeiter Ressourcen wie Holz, Stein und Erz"},
            { "BlackSmith.Desc","Hier erstellen die Arbeiter das ausgewählte Produkt"+_asLongHasInput},
            { "ShoreMine.Desc","Hier produzieren die Arbeiter Salz und Sand"},
            { "QuickLime.Desc","Hier werden die Arbeiter ungelöschten Kalk produzieren"},

            //Prod
            { "Brick.Desc","Hier wird ein Arbeiter Lehmprodukte wie Ziegelsteine usw. herstellen"},
            { "Carpentry.Desc","Hier produziert ein Arbeiter Holzprodukte wie Kisten, Fässer usw."},
            { "Cigars.Desc","Hier werden Arbeiter Zigarren produzieren"+_asLongHasInput},
            { "Mill.Desc","Hier werden die Arbeiter Korn mahlen"+_asLongHasInput},
            { H.Tailor+".Desc","Hier werden Arbeiter Kleidung herstellen"+_asLongHasInput},
            { "Tilery.Desc","Hier werden Arbeiter Dachziegel herstellen"+_asLongHasInput},
            { "Armory.Desc","Hier werden Arbeiter Waffen produzieren"+_asLongHasInput},
            { H.Distillery+".Desc",_produce},
            { "Chocolate.Desc",_produce},
            { "Ink.Desc",_produce},

            //Ind
            { "Cloth.Desc",_produce},
            { "GunPowder.Desc",_produce},
            { "PaperMill.Desc",_produce},
            { "Printer.Desc",_produce},
            { "CoinStamp.Desc",_produce},
            { "Silk.Desc",_produce},
            { "SugarMill.Desc",_produce},
            { "Foundry.Desc",_produce},
            { "SugarShop.Desc", "Stellt Produkte aus Zucker her!!!. " + _produce},


                { "SteelFoundry.Desc",_produce},

            //trade
            { "Dock.Desc","Hier kannst du Import- oder Exportaufträge hinzufügen (muss am Ozean platziert werden)." + _notRegionNeeded},
            { H.Shipyard + ".Desc","Hier können Schiffe repariert werden, aber es müssen die nötigen Reparaturmaterialien im Bestand sein"},
            { "Supplier.Desc","Hier werden Schiffe mit Waren be- und entladen, aber es müssen Artikel im Inventar haben, die ein Schiff für die lange Fahrt verwenden kann"},
            { "StorageSmall.Desc",_storage},
            { "StorageMed.Desc",_storage},
            { "StorageBig.Desc",_storage},
            { "StorageBigTwoDoors.Desc",_storage},
            { "StorageExtraBig.Desc",_storage},

            //gov
            { "Library.Desc","Die Leute kommen in dieses Gebäude, um Bücher zu lesen oder sie zu leihen um ihr Wissen zu erweitern. Je mehr Inventar in den Bibliotheken, desto besser"},
            { "School.Desc","Hier erhalten die Leute eine Ausbildung. Hier gilt mehr ist besser"},
            { "TradesSchool.Desc","Hier erhalten die Menschen eine spezialisierte Ausbildung im Handwerk. Gut ein paar Spezialisten zu haben"},
            { "TownHouse.Desc","Das Stadthaus erhöht die Zufriedenheit und den Wohlstand Ihres Volkes"},

            //other
            { "Church.Desc","Die Kirche gibt deinem Volk Glück und Hoffnung"},
            { "Tavern.Desc","Die Taverne bietet deinen Leuten Entspannung und Unterhaltung"},

            //Militar
            { "WoodPost.Desc", "Entdeckt Banditen und Piraten schneller, so dass du dich im Voraus darauf vorbereiten können"},
            { "PostGuard.Desc",_militar},
            { "Fort.Desc",_militar},
            { "Morro.Desc",_militar+". Sobald Sie dies gebaut haben, sollten Piraten es besser wissen"},

            //Decoration
            { "Fountain.Desc", "Verschönert deine Stadt und erhöht bei deinen Bürgern die allgemeine Zufriedenheit"},
            { "WideFountain.Desc", "Verschönert deine Stadt und erhöht bei deinen Bürgern die allgemeine Zufriedenheit"},
            { "PalmTree.Desc", "Verschönert deine Stadt und erhöht bei deinen Bürgern die allgemeine Zufriedenheit"},
            { "FloorFountain.Desc", "Verschönert deine Stadt und erhöht bei deinen Bürgern die allgemeine Zufriedenheit"},
            { "FlowerPot.Desc", "Verschönert deine Stadt und erhöht bei deinen Bürgern die allgemeine Zufriedenheit"},
            { "PradoLion.Desc", "Verschönert deine Stadt und erhöht bei deinen Bürgern die allgemeine Zufriedenheit"},



            //Buildings name
            //Infr
            { "Road","Straße"},
            { "BridgeTrail","Fußgängerbrücke"},
            { "BridgeRoad","Straßenbrücke"},
            { "LightHouse","Leuchtturm"},
            { "Masonry","Maurerbetrieb"},
            {   "StandLamp","Straßenlaterne"},
            { "HeavyLoad","Schwerlastbetrieb"},


            //House
            { "Shack", "Hütte"},
            { "MediumShack", "Mittelgroße Hütte"},
            { "LargeShack", "Große Hütte"},

            { "WoodHouseA", "Mittelgroßes Holzhaus" },
            { "WoodHouseB", "Großes Holzhaus"  },
            { "WoodHouseC", "Luxuriöses Holzhaus"},
            { "BrickHouseA", "Mittelgroßes Ziegelhaus"},
            { "BrickHouseB","Luxuriöses Ziegelhaus"},
            { "BrickHouseC","Großes Ziegelhaus"},

            
            //Farms
            //Animal
            { "AnimalFarmSmall","Kleine Tierfarm"},
            { "AnimalFarmMed","Mittelgroße Tierfarm"},
            { "AnimalFarmLarge","Große Tierfarm"},
            { "AnimalFarmXLarge","Extragroße Tierfarm"},
            //Fields
            { "FieldFarmSmall","Kleine Feldfarm"},
            { "FieldFarmMed","Mittelgroße Feldfarm"},
            { "FieldFarmLarge","Große Feldfarm"},
            { "FieldFarmXLarge","Extragroße Feldfarm"},
            { "FishingHut","Fischerhütte"},

            //Raw
            { "Mortar","Mörtel"},
            { "Clay","Lehm"},
            { "Pottery","Keramik"},
            { "MountainMine","Bergarbeitermine"},
            {  "LumberMill" ,"Sägewerk"},
            { "BlackSmith","Schmiede"},
            { "ShoreMine","Ufermine"},
            { "QuickLime","Üngelöschter Kalk"},

            //Prod
            { "Brick","Ziegel"},
            { "Carpentry","Zimmerei"},
            { "Cigars","Zigarren"},
            { "Mill","Mühle"},
            { "Tailor","Schneider"},
            { "Tilery","Fliesenleger"},
            { "Armory","Waffenkammer"},
            { "Distillery","Destillerie"},
            { "Chocolate","Schokolade"},
            { "Ink","Tinte"},

            //Ind
            { "Cloth","Kleidung"},
            { "GunPowder","Schießpulver"},
            { "PaperMill","Papiermühle"},
            { "Printer","Druckerei"},
            { "CoinStamp","Münzprägerei"},
            { "SugarMill","Zuckermühle"},
            { "Foundry","Gießerei"},
            { "SteelFoundry","Stahlgießerei"},
            { "SugarShop","Süßigkeitengeschäft"},


            //trade
            { "Dock","Dock"},
            { "Shipyard","Werft"},
            { "Supplier","Lieferant"},
            { "StorageSmall","Kleines Lager"},
            { "StorageMed","Mittelgroßes Lager"},
            { "StorageBig","Großes Lager"},

            //gov
            { "Library","Bücherei"},
            { "School","Schule"},
            { "TradesSchool","Handelsschule"},
            { "TownHouse","Stadthaus"},

            //other
            { "Church","Kirche"},
            { "Tavern","Taverne"},

            //Militar
            { "WoodPost", "Holzwachturm"},
            { "PostGuard","Steinwachturm"},
            { "Fort","Fort"},
            { "Morro", "Spanische Festung"},

            //Decorations
            { "Fountain", "Springrunnen"},
            { "WideFountain", "Großer Springbrunnen"},
            { "PalmTree", "Palme"},
            { "FloorFountain", "Brunnen"},
            { "FlowerPot", "Blumenkübel"},
            { "PradoLion", "Prado Löwe"},

            //Main GUI
            { "SaveGame.Dialog", "Speichere deinen Spielfortschritt"},
            { "LoadGame.Dialog", "Spiel laden"},
            { "NameToSave", "Speichere dein Spiel unter:"},
            { "NameToLoad", "Spiel zum Laden ausgewählt:"},
            { "OverWrite", "Es gibt bereits ein Spiel mit diesem Namen. Möchtest du die Datei überschreiben?"},
            { "DeleteDialog", "Möchtest du das gespeicherte Spiel wirklich löschen??"},
            { "NotHDDSpace", "Auf dem Laufwerk {0} ist nicht genügend Speicherplatz vorhanden, um das Spiel zu speichern"},
            { "GameOverPirate", "Das war´s leider, du hast das Spiel verloren! Piraten haben deine Stadt angegriffen und alle getötet."},
            { "GameOverMoney", "Tja aber sorry, du hast das Spiel verloren! Die Krone wird deine karibische Insel nicht mehr unterstützen."},
            { "BuyRegion.WithMoney", "Möchtest du diese Region wirklich kaufen?."},
            { "BuyRegion.WithOutMoney", "Sorry, das kannst du dir jetzt nicht leisten."},
            { "Feedback", "Feedback!? Super ... :) Danke. 8) "},
            { "OptionalFeedback", "Feedback ist mir sehr wichtig. Bitte lass ein paar Worte da."},
            { "MandatoryFeedback", "Das sieht nur das Entwicklerteam. Deine Bewertung ist?"},
            { "PathToSeaExplain", "Zeigt den kürzesten Weg zum Meer."},


            { "BugReport", "Einen Fehler gefunden? ähm, hoppla... schick es auf diesem Weg !! Vielen Dank"},
            { "Invitation", "Die E-Mail-Adresse deines Freundes, um an der Private Beta teilzunehmen"},
            { "Info", ""},//use for informational Dialogs
            { "Negative", "Die Krone hat dir einen Kreditrahmen gewährt. Wenn du mehr als $ 100.000,00 besitzt, ist das Spiel vorbei"},  


            //MainMenu
                { "Types_Explain", "Traditional: \nDas ist eine Spielvariante, bei dem am Anfang einige Gebäude gesperrt sind und man sie freischalten muss. " +
                    "Das Gute daran ist, dass du hier eine Anleitung bekommst." +
                    "\n\nFreewill: \nAlle verfügbaren Gebäude werden sofort freigeschaltet. " +
                    "Das Schlimme daran ist, dass du so leicht versagen kannst." +
                    "\n\nDie Stufe 'Schwer' ist verflucht nah der Realität"},


            //Tooltips
            //Small Tooltips
            { "Person.HoverSmall", "Gesamt/Erwachsene/Kinder"},
            { "Emigrate.HoverSmall", "Auswanderer"},
            { "CurrSpeed.HoverSmall", "Spieltempo"},
            { "Town.HoverSmall", "Stadtname"},
            { "Lazy.HoverSmall", "Arbeitslose Leute"},
            { "Food.HoverSmall", "Nahrung"},
            { "Happy.HoverSmall", "Zufriedenheit"},
            { "PortReputation.HoverSmall", "Ruf des Hafens"},
            { "Dollars.HoverSmall", "Dollar"},
            { "PirateThreat.HoverSmall", "Piratenbedrohung"},
            { "Date.HoverSmall", "Datum (Mmm/J)"},
            { "MoreSpeed.HoverSmall", "Schneller [BildHoch]"},
            { "LessSpeed.HoverSmall", "Langsamer [BildRunter]"},
            { "PauseSpeed.HoverSmall", "Spiel pausieren"},
            { "CurrSpeedBack.HoverSmall", "Aktuelle Geschwindigkeit"},
            { "ShowNoti.HoverSmall", "Benachrichtigungen"},
            { "Menu.HoverSmall", "Hauptmenü"},
            { "QuickSave.HoverSmall", "Schnellspeichern[Strg+S][F]"},
            { "Bug Report.HoverSmall", "Melde einen Fehler"},
            { "Feedback.HoverSmall", "Feedback"},
            { "Hide.HoverSmall", "Verbergen"},
            { "CleanAll.HoverSmall", "Sauber"},
            { "Bulletin.HoverSmall", "Kontrolle/Bulletin"},
            { "ShowAgainTuto.HoverSmall","Tutorial"},
            { "BuyRegion.HoverSmall", "Kaufe Regionen"},
            { "Help.HoverSmall", "Hilfe"},

            { "More.HoverSmall", "Mehr"},
            { "Less.HoverSmall", "Weniger"},
            { "Prev.HoverSmall", "Vorheriges"},

            { "More Positions.HoverSmall", "Mehr"},
            { "Less Positions.HoverSmall", "Weniger"},


            //down bar
            { "Infrastructure.HoverSmall", "Infrastruktur"},
            { "House.HoverSmall", "Häuser"},
            { "Farming.HoverSmall", "Landwirtschaft"},
            { "Raw.HoverSmall", "Güter"},
            { "Prod.HoverSmall", "Produktion"},
            { "Ind.HoverSmall", "Industrie"},
            { "Trade.HoverSmall", "Handel"},
            { "Gov.HoverSmall", "Regierung"},
            { "Other.HoverSmall", "Andere"},
            { "Militar.HoverSmall", "Militär"},
            { "Decoration.HoverSmall", "Dekoration"},

            { "WhereIsTown.HoverSmall", "Zurück zur Stadt [P]"},
            { "WhereIsSea.HoverSmall", "Ozeanpfad anzeigen"},
            { "Helper.HoverSmall", "Hilfe"},
            { "Tempeture.HoverSmall", "Temperatur"},
            
            //building window
            { "Gen_Btn.HoverSmall", "Allgemeiner Tab"},
            { "Inv_Btn.HoverSmall", "Inventar Tab"},
            { "Upg_Btn.HoverSmall", "Verbesserungen Tab"},
            { "Prd_Btn.HoverSmall", "Produktion Tab"},
            { "Sta_Btn.HoverSmall", "Statistiken Tab"},
            { "Ord_Btn.HoverSmall", "Befehle Tab"},
            { "Stop_Production.HoverSmall", "Produktion stoppen"},
            { "Demolish_Btn.HoverSmall", "Abreißen"},
            { "More Salary.HoverSmall", "Zahle mehr"},
            { "Less Salary.HoverSmall", "Zahle weniger"},
            { "Next_Stage_Btn.HoverSmall", "Nächste Stufe kaufen"},
            { "Current_Salary.HoverSmall", "Aktuelles Gehalt"},
            { "Current_Positions.HoverSmall", "Aktuelle Positionen"},
            { "Max_Positions.HoverSmall", "Maximale Positionen"},


            { "Add_Import_Btn.HoverSmall", "Import hinzufügen"},
            { "Add_Export_Btn.HoverSmall", "Export hinzufügen"},
            { "Upg_Cap_Btn.HoverSmall", "Verbesserungskapazität"},
            { "Close_Btn.HoverSmall", "Schließen"},
            { "ShowPath.HoverSmall", "Zeige Pfad"},
            { "ShowLocation.HoverSmall", "Standort anzeigen"},//TownTitle
            { "TownTitle.HoverSmall", "Stadt"},
            {"WarMode.HoverSmall", "Kampfmodus"},
            {"BullDozer.HoverSmall", "Bulldozer"},
            {"Rate.HoverSmall", "Bewerten"},

            //addOrder windiw
            { "Amt_Tip.HoverSmall", "Produktmenge"},

            //Med Tooltips 
            { "Build.HoverMed", "Gebäude platzieren: 'Linksklick' \n" +
                                "Gebäude drehen: 'R' Taste \n" +
                                "Abbrechen: 'Rechtsklick'"},
                { "BullDozer.HoverMed", "Gebiet bereinigen: 'Linksklick' \n" +
                "Abbruch: 'Rechtsklick' \nKosten: $10 pro Nutzung "},

                { "Road.HoverMed", "Start: 'Linksklick' \n" +
                    "Erweitern: 'Maus bewegen' \n" +
                    "Setzen: 'Nochmal Linksklick' \n" +
                "Abbruch: 'Rechtsklick'"},

            { "Current_Salary.HoverMed", "Die Arbeitnehmer gehen dort arbeiten wo das meiste Gehalt wartet." +
                                            " Wenn 2 Arbeitgeber das gleiche Gehalt zahlen nehmen sie den Job der ihrer Wohnung am nächsten ist."},



            //Notifications
            { "BabyBorn.Noti.Name", "Neugeborenes"},
            { "BabyBorn.Noti.Desc", "{0} ist geboren"},
            { "PirateUp.Noti.Name", "Piraten nähern sich"},
            { "PirateUp.Noti.Desc", "Piraten sind nahe dem Ufer"},
            { "PirateDown.Noti.Name", "Piraten respektieren dich"},
            { "PirateDown.Noti.Desc", "Piraten respektieren dich für heute ein wenig mehr"},

            { "Emigrate.Noti.Name", "Ein Bürger ist ausgewandert"},
            { "Emigrate.Noti.Desc", "Menschen wandern aus, wenn sie mit deiner Regierung nicht zufrieden sind"},
            { "PortUp.Noti.Name", "Hafen ist bekannt"},
            { "PortUp.Noti.Desc", "Der Ruf deines Hafens steigt mit den benachbarten Häfen und Routen"},
            { "PortDown.Noti.Name", "Hafen ist weniger bekannt"},
            { "PortDown.Noti.Desc", "Der Ruf deines Hafens ist gesunken"},

            { "BoughtLand.Noti.Name", "Neues Land gekauft"},
            { "BoughtLand.Noti.Desc", "Eine neue Landregion wurde gekauft"},

            { "ShipPayed.Noti.Name", "Schiff bezahlt"},
            { "ShipPayed.Noti.Desc", "Ein Schiff hat {0} für Waren oder Dienstleistungen bezahlt"},
            { "ShipArrived.Noti.Name", "Ein Schiff ist angekommen"},
            { "ShipArrived.Noti.Desc", "Ein neues Schiff ist in einem unserer Hafengebäude eingetroffen"},

            { "AgeMajor.Noti.Name", "Neuer Arbeiter"},
            { "AgeMajor.Noti.Desc", "{0} ist bereit für die Arbeit"},


            { "PersonDie.Noti.Name", "Jemand ist verstorben"},
            { "PersonDie.Noti.Desc", "{0} ist verstorben"},

            { "DieReplacementFound.Noti.Name", "Jemand ist verstorben"},
            { "DieReplacementFound.Noti.Desc", "{0} ist verstorben. Es wurde ein Ersatz für den Job gefunden."},

            { "DieReplacementNotFound.Noti.Name", "Jemand ist verstorben"},
            { "DieReplacementNotFound.Noti.Desc", "{0} ist verstorben. Es wurde kein Ersatz für den Job gefunden"},


            { "FullStore.Noti.Name", "Ein Lager wird voll"},
            { "FullStore.Noti.Desc", "Ein Lager ist bei einer Kapazität von {0}%"},

            { "CantProduceBzFullStore.Noti.Name", "Ein Arbeiter kann nichts herstellen"},
            { "CantProduceBzFullStore.Noti.Desc", "{0} weil sein/ihr Ziellager voll ist"},

            { "NoInput.Noti.Name", "Zumindest fehlt eine Zufuhr zum Gebäude"},
            { "NoInput.Noti.Desc", "Ein Gebäude kann keine {0} erzeugen, da mindestens ein Rohstoff fehlt"},

            { "Built.Noti.Name", "Ein Gebäude wurde fertiggestellt"},
            { "Built.Noti.Desc", "{0} wurde fertiggestellt"},

            { "cannot produce", "kann nicht herstellen"},

            



            //Main notificaion
            //Shows on the middle of the screen
            { "NotScaledOnFloor", "Das Gebäude liegt entweder zu nahe am Ufer oder zu nah am Berg"},
            { "NotEven", "Der Boden unter dem Gebäude ist nicht eben"},
            { "Colliding", "Gebäude kollidiert mit einem anderen"},
            { "Colliding.BullDozer", "Bulldozer kollidiert mit einem Gebäude. Kann nur im Gelände (Bäume, Felsen) verwendet werden"},

            { "BadWaterHeight", "Das Gebäude ist zu tief oder zu hoch auf dem Wasser"},
            { "LockedRegion", "Du musst diese Region besitzen, um hier bauen zu können"},
            { "HomeLess", "Die Leute in diesem Haus können nirgendwo hin gehen. Bitte baue ein neues Haus" +
                            " um diese Familie zu halten und es erneut versuchen"},
            { "LastFood", "Kann nicht zerstört werden, dies ist das einzige Lager in deinem Dorf"},
            { "LastMasonry", "Kann nicht zerstört werden, dies ist der einzige Maurer in deinem Dorf"},
            { "OnlyOneDemolish", "Du zerstörst bereits ein Gebäude. Versuche es erneut, nachdem der Abbruch abgeschlossen ist"},


            //help

            { "Construction.HoverMed", "Für den Bau eines Gebäudes benötigst du Arbeiter im Maurerbetrieb. "+
                    " Klicke auf den Maurer und dann auf das Symbol "+" auf der Registerkarte 'Allgemein'. Stelle sicher, dass du über ausreichende Ressourcen verfügst"},
            { "Demolition.HoverMed", "Sobald die Vorräte frei sind, wird es abgerissen. Schubkarren transportieren die Reste ab"},

            { "Construction.Help", "Für den Bau eines Gebäudes benötigen Sie Arbeiter im Maurerbetrieb. "+
                    " Klicke auf den Maurer und dann auf das Symbol "+" auf der Registerkarte 'Allgemein'. Stelle sicher, dass du über ausreichende Ressourcen verfügst"},
            { "Camera.Help", "Kamera: Benutze [WASD] oder den Cursor zum bewegen. " +
                        "Drücke das Scrollrad deiner Maus und halte es gedrückt, um zu drehen, oder nutze [Q] und [E]"},
            { "Sea Path.Help", "Klicke auf die linke untere Ecke 'Seepfad ein- / ausblenden' " +
                            "Schaltfläche, um den nächstgelegenen Weg zum Meer anzuzeigen"},

            { "People Range.Help", "Der große blaue Kreis um jedes Gebäude markiert die Reichweite"},

            { "Pirate Threat.Help", "Piratenbedrohung: So bewusst sind sich die Piraten der Existenz deines Hafens. Diese erhöht sich," +
                                        " wenn du mehr Geld besitzt. Wenn dieser Wert über 90 erreicht wird, verlierst du das Spiel. Du kannst der Bedrohung durch den Bau militärischer Gebäude begegnen"},

            { "Port Reputation.Help", "Ruf des Hafens: Je mehr Leute deinen Hafen kennen, desto mehr besuchen ihn." +
                                            " Wenn du dies verbessern möchtest, stelle sicher, dass du immer einige Aufträge" +
                                            " im Dock hast"},
            { "Emigrate.Help", "Auswanderer: Wenn die Menschen einige Jahre unglücklich sind, gehen sie. Das Schlechte" +
                                    " daran ist, dass sie nicht wiederkommen, keine Kinder bekommen oder Kinder haben werden." +
                                    " Das einzig Gute ist, dass sie den 'Ruf des Hafens' erhöhen."},
            { "Food.Help", "Lebensmittel: Je höher die Vielfalt an Lebensmitteln, die in einem Haushalt verfügbar sind, desto zufriedener "+
                                 " werden sie sein."},

            { "Weight.Help", "Gewicht: Alle Gewichtsangaben im Spiel sind in kg oder Pfund, abhängig davon, welches Einheitensystem ausgewählt ist." +
                                " Du kannst dies unter 'Optionen' im 'Hauptmenü' ändern."},
            { "What is Ft3 and M3?.Help", "Die Speicherkapazität wird durch das Volumen des Gebäudes bestimmt. Ft3 ist ein Kubikfuß. M3 ist ein Kubikmeter" },//. Denke daran, dass weniger kompakte Produkte dein Lager schnell füllen. Die Produktmenge findest du unter  Bulletin/Prod/Spezi"},

            { "More.Help", "Wenn du weitere Hilfe benötigst, empfiehlt es sich, das Tutorial abzuschließen oder einfach deine Frage in den SugarMill-Foren zu posten"},

                //more 
            { "Products Expiration.Help", "Produktverfall: Wie in der Realität verfällt jedes Produkt. Einige Lebensmittel verfallen früher als andere. Du kannst unter Bulletin/Prod/Verfall sehen welche Produkte abgelaufen sind"},
            { "Horse Carriages.Help", "Da das Spiel echte Maße hat, können die Menschen nur begrenzt viel tragen. Danach kommen Pferdefuhrwerk zum Einsatz. Sie transportieren viel mehr, wodurch deine Wirtschaft gestärkt wird. Eine Person in ihren besten Jahren könnte etwa 15 kg, eine Schubkarre ca. 60 kg und der kleinere Wagen kann 240 kg tragen. Um diese zu benutzen, baue eine Schwertransportstation"},
            { "Usage of goods.Help", "Verwendung von Gütern: Kisten, Fässer, Schubkarren, Karren, Werkzeuge, Kleidung, Geschirr, Möbel und Utensilien sind erforderlich, um die traditionellen Aktivitäten einer Stadt zu erledigen. Wenn diese Waren verwendet werden, verringert sich ihre Anzahl, so dass eine Person nichts trägt, wenn keine Kisten vorhanden sind. Behalte das im Auge ;)"},
            { "Happiness.Help", "Zufriedenheit: Die Zufriedenheit der Menschen wird von verschiedenen Faktoren beeinflusst. Wie viel Geld sie haben, der Lebensmittelvielfalt, Religionszufriedenheit, Zugang zu Freizeit, Hauskomfort und Bildungsniveau. Auch wenn eine Person Zugang zu Utensilien hat, beeinflussen eher Geschirr und Kleidung ihre Zufriedenheit."},
            { "Line production.Help", "Serienfertigung: Um einen einfachen Nagel herzustellen, muss man Erz abbauen, in der Gießerei das Eisen schmelzen und schließlich muss die Schmiede den Nagel herstellen. Wenn du genug Geld hast, kannst du den Nagel immer direkt auf einem Schiff kaufen wie auch jedes andere Produkt."},
            { "Bulletin.Help", "Das Seitensymbol in der unteren Leiste ist das Bulletin / Kontrollfenster. Bitte nimm dir etwas Zeit, um es zu erkunden."},
            { "Trading.Help", "Du benötigst mindestens ein Dock, um handeln zu können. Dort kannst du Import- / Exportaufträge hinzufügen und Bargeld verdienen. Wenn du beim Hinzufügen eines Auftrages Hilfe benötigst, kannst du das Tutorial abschließen"},

            { "Combat Mode.Help", "Wird aktiviert, wenn ein Pirat/ Bandit von einem deiner Bürger entdeckt wird. Sobald der Modus aktiv ist, kannst du Einheiten direkt den Angriff zuweisen. Wähle sie aus und klicke mit der rechten Maustaste, um das Ziel anzugreifen"},

            { "Population.Help", "Sobald sie 16 Jahre alt sind, ziehen sie in ein freies Haus, wenn eines zur Verfügung steht. Immer wenn ein freies Haus zur Verfügung steht, wird das Bevölkerungswachstum garantiert. Wenn sie mit 16 Jahren in die neuen Häuser kommen, maximieren Sie das Bevölkerungswachstum"},


            { "F1.Help", "Drücke [F1] für die Hilfe"},

            { "Inputs.Help", "Wenn ein Gebäude nichts produzieren kann, weil die Waren fehlen, prüfe bitte ob die erforderlichen Güter im Hauptlager vorhanden sind und mindestens eine Arbeiter im Maurerbetrieb ist"},
            { "WheelBarrows.Help", "Schubkarren sind die 'Arbeitstiere' der Maurer. Wenn sie nichts zu bauen haben, fungieren sie als Schubkarren. Wenn du Materialien benötigst, die in ein bestimmtes Gebäude gelangen müssen, stelle sicher, dass du über genügend Einheiten verfügst und auch die im Lagergebäude genannten Materialien"},

            { "Production Tab.Help", "Wenn es sich bei dem Gebäude um ein landwirtschaftliches Feld handelt, stelle sicher, dass sich auf dem Hof Arbeiter befinden. Die Ernte geht verloren, wenn sie einen Monat nach dem Erntetag noch immer dort lagert"},
            { "Our Inventories.Help", "TDer Abschnitt 'Unsere Lagerbestände' im Fenster 'Auftrag hinzufügen' ist eine Zusammenfassung dessen, was wir aus den Lagerbeständen unserer Lagergebäude erhalten haben"},
            { "Inventories Explanation.Help", "Dies ist eine Zusammenfassung dessen, was wir in unseren Lagerbeständen haben. Gegenstände in anderen Gebäudebeständen gehören nicht zur Stadt"},

            ///word and grammarly below




            //to  add on spanish         //to correct  
            { "TutoOver", "Deine Belohnung beträgt $ 10.000,00, wenn du es zum ersten Mal abschließt. Das Tutorial ist nun beendet. Jetzt kannst dieses Spiel entweder weiterspielen oder ein neues Spiel beginnen."},

            //Tuto
            { "CamMov.Tuto", "Die Belohnung für den Abschluss des Tutorials beträgt $10.000, - (einmalige Belohnung pro Spiel). Schritt 1: Bewege die Kamera mit den Tasten [WASD] oder den Pfeiltasten. Tue dies mindestens 5 Sekunden lang"},
            { "CamMov5x.Tuto", "Verwende die [WASD] oder die Pfeiltasten und halte die linke Umschalttaste gedrückt, um die Kamera fünfmal schneller zu bewegen. Tue dies mindestens 5 Sekunden lang"},
            { "CamRot.Tuto", "Drücke nun das Mausrad nach unten und bewege die Maus, um die Kamera zu drehen. Tue dies mindestens 5 Sekunden lang"},


            { "BackToTown.Tuto", "PDrücke die Taste [P] auf der Tastatur, um zur Ausgangsposition der Kamera zu gelangen"},

            { "BuyRegion.Tuto", "Regionen musst du besitzen, um bauen zu können. Klicke auf das "+" - Zeichen in der unteren Leiste und dann auf das 'Zum Verkauf' -Symbol in der" +
                    " Mitte einer Region, um es zu kaufen. Einige Gebäude sind ausgenommen, sie können gebaut werden, ohne die Region zu besitzen" +
                    " (Fischerhütte, Dock, Bergwerk, Uferbergwerk, Leuchtturm, Wachposten)"
                    },

            { "Trade.Tuto", "Das war echt einfach, aber der schwierige Teil kommt jetzt. Klicke auf die Schaltfläche 'Handelsgebäude' in der rechten unteren Leiste. "+
                "Wenn du den Mauszeiger darüber bewegst, erscheint ein Popup-Fenster namens 'Handel'."},
            { "CamHeaven.Tuto", "Scrolle mit der mittleren Maustaste zurück, bis die Kamera den"
                    + " Himmel erreicht. Diese Ansicht ist nützlich, um größere Gebäude wie den Hafen zu platzieren"},

            { "Dock.Tuto", "Klicke nun auf das 'Dock'-Gebäude, es ist der 1. Button. Wenn du darüber schwebbleibst, zeigt es"+
                "die Kosten und die Beschreibung"},
            { "Dock.Placed.Tuto", "Nun zum schweren Teil, daher sorgfältig lesen. Beachte, dass du die "+
                "'R' Taste zum rotieren nutzen kannst und den Rechtklick zum Bauabbruch. Dieses Gebäude hat einen Teil im Ozean und einen anderen an Land." +
                " Der Pfeil zeigt zum Meer, der Lagerbereich ist an Land. Wenn der Pfeil weiß ist, klicke mit der linken Maustaste."},
            { "2XSpeed.Tuto", "Erhöhe die Geschwindigkeit des Spiels, gehe zum mittleren Geschwindigkeitsregler für die Simulation auf dem oberen Bildschirm und klicke auf die"
                    +" 'Schneller' Schaltfläche 1 Mal bis 2x angezeigt wird"},

            { "ShowWorkersControl.Tuto", "Klicke auf die Schaltfläche 'Steuerung/ Bulletin' in der unteren Leiste. "+
                "Wenn du den Mauszeiger darüber bewegst, wird 'Steuerung / Bulletin' angezeigt.. "},
            { "AddWorkers.Tuto", "Klicke auf das "+" - Zeichen rechts neben dem Maurerbetrieb. So fügst du weitere Arbeiter hinzu."},
            { "HideBulletin.Tuto", "Denke bitte daran, dass du in diesem Fenster verschiedene Aspekte des Spiels steuern und sehen kannst. Klicke außerhalb des Fensters, um es zu schließen, oder auf die Schaltfläche 'OK'."},
            { "FinishDock.Tuto", "Beende nun den Dockbau. Je mehr Arbeiter im Maurerbetrieb sind, desto schneller wird die Arbeit auch erledigt."
            + " Vergewissere dich auch, dass du über alle Materialien verfügst, die für den Bau erforderlich sind"},
            { "ShowHelp.Tuto", "Klicke auf die Schaltfläche 'Hilfe' in der unteren Leiste. "+
                "Wenn du den Mauszeiger darüber bewegst, wird die Hilfe angezeigt. Dort findest du einige hilfreiche Tipps."},


            { "SelectDock.Tuto", "Schiffe legen Waren nach dem Zufallsprinzip aus dem Inventar des Docks ab. Arbeiter werden benötigt, um Güter auf dem Dock ein- und auszulagern. Sie benötigen Schubkarren und Kisten. Wenn sich keine dieser Materialien im Docklager befinden, arbeiten sie nicht. Klicke nun auf das Dock."},


            { "OrderTab.Tuto", "Wechsele zur Registerkarte 'Bestellungen' im Dockfenster."},
            { "ImportOrder.Tuto", "Klicke auf das '+' neben Importauftrag hinzufügen."},

            { "AddOrder.Tuto", "Scrolle nun in den Produkten nach unten und wähle Holz aus, und gib als Menge 100 ein. Klicke dann auf die Schaltfläche 'Hinzufügen'."},
            { "CloseDockWindow.Tuto", "Nun wird die Bestellung hinzugefügt. Ein zufälliges Schiff legt diese Gegenstand in das Dockinventar. Ihre Dockarbeiter werden es dann zum nächstgelegenen Lagergebäude bringen. Klicke jetzt außerhalb des Fensters, damit es geschlossen wird."},
            { "Rename.Tuto", "Klicke auf eine Person und dann auf die Titelleiste der Person. So kannst du den Namen einer beliebigen Person oder eines Gebäudes im Spiel ändern. Klicke außerhalb, um die Änderung zu speichern"},
            { "RenameBuild.Tuto", "Klicke nun auf ein Gebäude und ändere seinen Namen auf dieselbe Weise. Denke daran, außerhalb zu klicken, damit die Änderung gespeichert wird"},

            { "BullDozer.Tuto", "Klicke nun auf das Bulldozer-Symbol in der unteren Leiste. Dann entferne einen Baum oder einen Stein vom Gelände."},


            { "Budget.Tuto", "Klicke auf die Schaltfläche 'Steuerung / Bulletin', dann auf das Menü 'Finanzen' und dann auf 'Hauptbuch'. Dies ist das Spielkonto"},
            { "Prod.Tuto", "Klicke auf 'Prod' und dann auf 'Produziert'. Dies zeigt die Produktion des Dorfes der letzten 5 Jahre"},
            { "Spec.Tuto", "Klicke auf das Menü 'Prod' und dann auf 'Spez'. Hier kannst du genau sehen, wie du jedes Produkt im Spiel herstellst. Die notwendigen Eingänge und wo wird produziert. Auch die Import- und Exportpreise"},
            { "Exports.Tuto", "Klicke auf das Menü 'Finanzen' und dann auf 'Exportieren'. Hier siehst eine Aufschlüsselung der Exporte deines Dorfes"},


                //Quest
            { "Tutorial.Quest", "Quest: Beende das Tutorial. Belohnung 10.000 $. Dies dauert ungefähr 3 Minuten"},

            { "Lamp.Quest", "Quest: Baue eine Straßenlaterne. Du findest sie in der Infrastruktur. Sie leuchtet nachts, wenn Walöl im Lager ist"},

            { "Shack.Quest", "Quest: Baue eine Hütte. Das sind die billigsten Häuser. Wenn Menschen 16 Jahre alt werden, ziehen sie in ein freies Haus, wenn sie eines finden. Auf diese Weise wird das Bevölkerungswachstum garantiert. [F1] Hilfe. Wenn du Rauch im Schornstein siehst, bedeutet das, dass Menschen darin leben"},

            { "SmallFarm.Quest", "Quest: Baue eine kleine Feldfarm. Du benötigst Farmen, um deine Leute zu ernähren"},
            { "FarmHire.Quest", "Quest: Stellen Sie zwei Bauern auf der kleinen Feldfarm ein. Klicke auf die Farm und klicke das Pluszeichen um Arbeiter einzustellen. Arbeitlos sollten sie aber schon sein"
                    +" um sie einem Gebäude zuweisen zu können"},



            { "FarmProduce.Quest", "Quest: Produziere " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " Bohnen auf einer kleinen Feldfarm. Klicke auf die Registerkarte 'Stat' und zeige die Produktion der letzten 5 Jahre an. Du kannst den Questfortschritt im Questfenster sehen. Wenn du weitere kleine Farmen baust, werden diese für die Quest berücksichtigt"},
            { "Transport.Quest", "Quest: Transportiere nun die Bohnen vom Feld zum Lager. Um dies tun zu können" +
                " solltest du genügend Arbeiter frei haben. Sie fungieren als Transporteure, wenn sie nichts bauen"},


            { "HireDocker.Quest", "Quest: Einen Hafenarbeiter anstellen. Die Aufgabe von Hafenarbeitern besteht nur darin, die Waren aus dem Lager in das Dock zu verschieben, wenn sie exportiert werden sollen."+
            " Natürlich auch umgekehrt beim Import. Sie arbeiten, wenn eine Bestellung vorliegt und die Ware transportbereit ist. Ansonsten bleiben sie zu Hause und ruhen sich aus." +
                " Wie du hast noch kein Dock gebaut? Dann wird´s aber Zeit ein zu bauen."+
            " Du findest es unter Handel." },


            { "Export.Quest", "Quest: Erstelle im Dock einen Auftrag und exportiere genau 300 " + Unit.CurrentWeightUnitsString() + " Bohnen."+
                " Klicke im Dock auf die Registerkarte 'Bestellungen' und füge einen Exportauftrag mit dem "+" - Zeichen hinzu."+
            " Produkt auswählen und Betrag eingeben"},



            { "MakeBucks.Quest", "Quest: Verdiene $ 100 beim Export von Waren im Dock. "+
            "Sobald ein Schiff ankommt, werden die Produkte nach dem Zufallsprinzip im Inventar Ihres Docks bezahlt"},
            { "HeavyLoad.Quest", "Quest: Baue ein Schwerlastgebäude. Dies sind Spediteure, die mehr Gewicht tragen können. Sie werden nützlich sein, wenn der Transport von Gütern erforderlich ist." }, //In den Lagern der Städte müssen Karren vorhanden sein, damit sie arbeiten können"},
            { "HireHeavy.Quest", "Quest: Im Schwerlastgebäude einen Schwerlasttransporteur einstellen."},


            { "ImportOil.Quest", "Quest: Importiere 500 " + Unit.CurrentWeightUnitsString() + " Walöl über das Dock. Dies ist erforderlich, um Lichter nachts eingeschaltet zu lassen. Schiffe werden Importe zufällig im Inventar Ihres Dock ablegen"},

            { "Population50.Quest", "Erreiche eine Gesamtbevölkerung von 50 Einwohnern"},

            //added Aug 11 2017, result: sep 9(30% off biggest sale ever)
            { "Production.Quest", "Lass uns jetzt ein paar Waffen herstellen und später verkaufen. Baue zuerst einen Schmied. Diesen findest du im 'Güter' Gebäudemenü"},
            { "ChangeProductToWeapon.Quest", "Im 'Produkte Tab' des Schmieds änderst du die Produktion in Waffen. Die Arbeiter bringen das Rohmaterial mit, wenn es vorrätig ist, um Waffen zu schmieden"},
            { "BlackSmithHire.Quest", "Stelle zwei Schmiedegesellen ein"},
            { "WeaponsProduce.Quest", "Nun produziere " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " Waffen in der Schmiede. Klicke auf die Registerkarte 'Stat' und zeige die Produktion der letzten 5 Jahre an. Du kannst den Questfortschritt im Questfenster verfolgen."},
            { "ExportWeapons.Quest", "Nun exportiere 100 " + Unit.CurrentWeightUnitsString() + " Waffen. Füge im Dock eine Bestellung für den Export hinzu. Denke stets daran dass Waffen ein sehr profitables Geschäft sind"},


            {"CompleteQuest", "Deine Belohnung ist {0}"},


            //added Sep 14 2017
            { "BuildFishingHut.Quest", "Baue eine Fischerhütte. Auf diese Weise haben die Bürger verschiedene Nahrungsmittel zur Verfügung, was sich in Zufriedenheit niederschlägt"},
            { "HireFisher.Quest", "Stelle einen Fischer ein"},

            { "BuildLumber.Quest", "Baue eine Holzfällermühle. Du findest sie im Gebäude-Menü unter 'Güter'"},
            { "HireLumberJack.Quest", "Stelle einen Holzfäller ein"},

            { "BuildGunPowder.Quest", "Baue eine Schießpulverfabrik. Du findest sie im Gebäude-Menü unter 'Industrie'"},
            { "ImportSulfur.Quest", "Importiere bei den Docks 1000 " + Unit.CurrentWeightUnitsString() + " Schwefel"},
            { "GunPowderHire.Quest", "Stelle einen Arbeiter für die Schießpulverfabrik ein"},

            { "ImportPotassium.Quest", "Importiere bei den Docks 1000 " + Unit.CurrentWeightUnitsString() + " Kalium"},
            { "ImportCoal.Quest", "Importiere bei den Docks 1000 " + Unit.CurrentWeightUnitsString() + " K"},

            { "ProduceGunPowder.Quest", "Produziere nun " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " Schießpulver. Beachte, dass du Schwefel, Kalium und Kohle benötigst, um Schießpulver herzustellen"},
            { "ExportGunPowder.Quest", "Exportiere bei den Docks 100 " + Unit.CurrentWeightUnitsString() + " Schießpulver"},

            { "BuildLargeShack.Quest", "Baue eine große Hütte. In diesen größeren Häusern  wird die Bevölkerung schneller wachsen"},

            { "BuildA2ndDock.Quest", "Baue ein zweites Dock. Dieses Dock kann nur für Importe verwendet werden. Auf diese Weise kannst du Rohstoffe hier importieren und an einem anderen Dock exportieren"},
            { "Rename2ndDock.Quest", "Benenne die Docks jetzt um, so dass du leichter erkennst, welche nur für Importe und welche für Exporte verwendet werden"},

            { "Import2000Wood.Quest", "Imporitier nun auf dem Importdock 2000 " + Unit.CurrentWeightUnitsString() + " Holz. Dieses Rohmaterial wird für alles benötigt, allerdings auch als Brennstoff"},

            //IT HAS FINAL MESSAGE 
            //last quest it has a final message to the player. if new quest added please put the final message in the last quest
            { "Import2000Coal.Quest", "Imortiere nun im Importdock 2000 " + Unit.CurrentWeightUnitsString() + " Kohle. Kohle wird auch für alles benötigt, weil sie als Brennstoff verwendet wird. Ich hoffe, du genießt die bisherige Spielerfahrung. Erweitere deine Kolonie und deinen Wohlstand. Bitte hilf auch mit das Spiel zu verbessern. Beteilige dich an unserem Online-Forum, denn deine Stimme und deine Meinung sind wichtig! Viel Spaß Sugarmiller!"},

            //



            //Quest Titles
            { "Tutorial.Quest.Title", "Tutorial"},
            { "Lamp.Quest.Title", "Straßenlaterne"},

            { "Shack.Quest.Title", "Baue eine Hütte"},
            { "SmallFarm.Quest.Title", "Baue ein Farmfeld"},
            { "FarmHire.Quest.Title", "Stelle zwei Farmer ein"},


            { "FarmProduce.Quest.Title", "Landwirtschaftlicher Produzent"},

            { "Export.Quest.Title", "Exporte"},
            { "HireDocker.Quest.Title", "Stelle einen Dockarbeiter ein"},
            { "MakeBucks.Quest.Title", "Verdiene Geld"},
            { "HeavyLoad.Quest.Title", "Schwerlasttransport"},
            { "HireHeavy.Quest.Title", "Stelle einen Schwerlasttransporteur ein"},

            { "ImportOil.Quest.Title", "Walöl"},

            { "Population50.Quest.Title", "50 Einwohner"},
            
            //
            { "Production.Quest.Title", "Stelle Waffen her"},
            { "ChangeProductToWeapon.Quest.Title", "Ändere ein Produkt"},
            { "BlackSmithHire.Quest.Title", "Stelle zwei Schmiedegesellen ein"},
            { "WeaponsProduce.Quest.Title", "Schmiede Waffen"},
            { "ExportWeapons.Quest.Title", "Mach ordentlich Profit" },
            
            //
            { "BuildFishingHut.Quest.Title", "Baue eine Fischerhütte"},
            { "HireFisher.Quest.Title", "Stelle einen Fischer ein"},
            { "BuildLumber.Quest.Title", "Baue eine Sägemühle"},
            { "HireLumberJack.Quest.Title", "Stelle einen Holzfäller ein"},
            { "BuildGunPowder.Quest.Title", "Baue eine Schießpulverfabrik"},
            { "ImportSulfur.Quest.Title", "Imortiere Schwefel"},
            { "GunPowderHire.Quest.Title", "Stelle einen Schießpulverfabrikarbeiter ein"},
            { "ImportPotassium.Quest.Title", "Importiere Kalium"},
            { "ImportCoal.Quest.Title", "Importiere Kohle"},
            { "ProduceGunPowder.Quest.Title", "Stelle Schießpulver her"},
            { "ExportGunPowder.Quest.Title", "Exortiere Schießpulver"},
            { "BuildLargeShack.Quest.Title", "Baue eine große Hütte"},
            { "BuildA2ndDock.Quest.Title", "Baue ein zweites Dock"},
            { "Rename2ndDock.Quest.Title", "Benenne das zweite Dock um"},
            { "Import2000Wood.Quest.Title", "Importiere etwas Holz"},
            { "Import2000Coal.Quest.Title", "Importiere etwas Kohle"},




            {"Tutorial.Arrow", "Dies ist das Tutorial. Sobald du es abgeschlossen hast, kannst du 10.000 $ einsacken"},
            {"Quest.Arrow", "Dies ist der Quest-Button. Du kannst hier auf das Questfenster zugreifen, indem du darauf klickst"},
            {"New.Quest.Avail", "Mindestens eine Quest ist verfügbar"},
            {"Quest_Button.HoverSmall", "Quest"},



            //Products
            //Notification.Init()
            {"RandomFoundryOutput", "Geschmolzenes Erz"},

            //OrderShow.ShowToSetCurrentProduct()
            { "RandomFoundryOutput (Ore, Wood)", "Geschmolzenes Erz (Erz, Holz)"},



            //Bulleting helps
            {"Help.Bulletin/Prod/Produce", "Hier wird angezeigt, was im Dorf produziert wird."},
            {"Help.Bulletin/Prod/Expire", "Hier wird angezeigt, was im Dorf an Waren verfallen ist."},
            {"Help.Bulletin/Prod/Consume", "Hier wird angezeigt, was von deinen Leuten verbraucht wird."},

            {"Help.Bulletin/Prod/Spec", "In diesem Fenster kannst du die für jedes Produkt erforderlichen Materialien sehen, wo es hergestellt wird und den Preis. "
            + "Scrolle nach oben, um die Kopfzeilen anzuzeigen. Beachte dass ein einfaches Produkt mehr als nur ein Grundprodukt sein kann."},

            {"Help.Bulletin/General/Buildings", "Dies ist eine Zusammenfassung wie viele Gebäude von jedem Typ es gibt."},

            {"Help.Bulletin/General/Workers", "In diesem Fenster können Arbeiter für verschiedene Gebäude zugewiesen werden. "
            + "Damit ein Gebäude mehr Menschen Arbeit bieten kann, muss es unter der Kapazität liegen und muss mindestens einen Arbeitslosen finden."},

            {"Help.Bulletin/Finance/Ledger", "Here is shown your ledger. Salary is the amount of money paid to a worker. The more people working the more salary will be paid out."},
            {"Help.Bulletin/Finance/Exports", "Ein Zusammenbruch der Exporte"},
            {"Help.Bulletin/Finance/Imports", "Ein Zusammenbruch der Importe"},


            { "Help.Bulletin/Finance/Prices", "...."},


            {"LoadWontFit", "Diese Ladung passt nicht in den Lagerbereich"},

            {"Missing.Input", "Gebäude kann nicht produzieren (Materialien müssen sich im Gebäudeinventar befinden). Fehlende Materialien: \n" },





            //in game
            
            { "Buildings.Ready", "\n Gebäude fertig zum Bauen:"},
            { "People.Living", "Menschen, die in diesem Haus leben:"},
            { "Occupied:", "Gefüllt:"},
            { "|| Capacity:", "|| Kapazität:"},
            { "Users:", "\nUsers:"},
            { "Amt.Cant.Be.0", "Menge kann nicht 0 sein "},
            { "Prod.Not.Select", "Bitte wähle ein Produkt"},


            //articles
            { "The.Male", "Der"},
            { "The.Female", "Die"},

            //
            { "Build.Destroy.Soon", "Dieses Gebäude wird bald zerstört. Wenn das Inventar nicht leer ist, muss es mit Schubkarren abtransportiert werden"},




            //words
            //Field Farms
            { "Bean", "Bohnen"},
            { "Potato", "Kartoffeln"},
            { "SugarCane", "Zuckerrohr"},
            { "Corn", "Getreide"},
            { "Cotton", "Baumwolle"},
            { "Banana", "Bananen"},
            { "Coconut", "Kokosnüsse"},
            //Animal Farm
            { "Chicken", "Hühner"},
            { "Egg", "Eier"},
            { "Pork", "Schweine"},
            { "Beef", "Rindfleisch"},
            { "Leather", "Leder"},
            { "Fish", "Fisch"},
            //mines
            { "Gold", "Gold"},
            { "Stone", "Stein"},
            { "Iron", "Eisen"},

            // { "Clay", "Lehm"},
            { "Ceramic", "Keramik"},
            { "Wood", "Holz"},

            //Prod
            { "Tool", "Werkzeuge"},
            { "Tonel", "Ton"},
            { "Cigar", "Zigarren"},
            { "Tile", "Ziegel"},
            { "Fabric", "Fabrik"},
            { "Paper", "Papier"},
            { "Map", "Karte"},
            { "Book", "Buch"},
            { "Sugar", "Zucker"},
            { "None", "Nichts"},
            //
            { "Person", "Person"},
            { "Food", "Nahrung"},
            { "Dollar", "Dollar"},
            { "Salt", "Salz"},
            { "Coal", "Kohle"},
            { "Sulfur", "Schwefel"},
            { "Potassium", "Kalium"},
            { "Silver", "Silber"},
            { "Henequen", "Agave"},
            //
            { "Sail", "Segel"},
            { "String", "Seil"},
            { "Nail", "Nägel"},
            { "CannonBall", "Kanonenkugel"},
            { "TobaccoLeaf", "Tabakblatt"},
            { "CoffeeBean", "Kaffebohne"},
            { "Cacao", "Kakao"},
            { "Weapon", "Waffe"},
            { "WheelBarrow", "Schubkarre"},
            { "WhaleOil", "Walöl"},
            //
            { "Diamond", "Diamanten"},
            { "Jewel", "Juwelen"},
            { "Rum", "Rum"},
            { "Wine", "Wein"},
            { "Ore", "Erz"},
            { "Crate", "Kiste"},
            { "Coin", "Münzen"},
            { "CannonPart", "Kanonenteil"},
            { "Steel", "Stahl"},
            //
            { "CornFlower", "Sonnenblume"},
            { "Bread", "Brot"},
            { "Carrot", "Karotte"},
            { "Tomato", "Tomate"},
            { "Cucumber", "Gurke"},
            { "Cabbage", "Kohl"},
            { "Lettuce", "Grüner Salat"},
            { "SweetPotato", "Süßkartoffel"},
            { "Yucca", "Yucca"},
            { "Pineapple", "Ananas"},
            //
            { "Papaya", "Papaya"},
            { "Wool", "Wolle"},
            { "Shoe", "Schuh"},
            { "CigarBox", "Zigarrenkiste"},
            { "Water", "Wasser"},
            { "Beer", "Bier"},
            { "Honey", "Honig"},
            { "Bucket", "Eimer"},
            { "Cart", "Wagen"},
            { "RoofTile", "Dachteil"},
            { "FloorTile", "Bodenteil"},
            { "Furniture", "Möbel"},
            { "Crockery", "Geschirr"},

            { "Utensil", "Utensilien"},
            { "Stop", "Stop"},


            //more Main GUI
            { "Workers distribution", "Arbeiterverteilung"},
            { "Buildings", "Gebäude"},

            { "Age", "Alter"},
            { "Gender", "Geschlecht"},
            { "Height", "Größe"},
            { "Weight", "Gewicht"},
            { "Calories", "Kalorien"},
            { "Nutrition", "Ernährung"},
            { "Profession", "Beruf"},
            { "Spouse", "Ehepartner"},
            { "Happinness", "Zufriedenheit"},
            { "Years Of School", "Schuljahre"},
            { "Age majority reach", "Alter mehrheitlich erreicht"},
            { "Home", "Zuhause"},
            { "Work", "Arbeit"},
            { "Food Source", "Nahrungsquellen"},
            { "Religion", "Religion"},
            { "Chill", "Ausgeruhtheit"},
            { "Thirst", "Durst"},
            { "Account", "Konto"},

            { "Early Access Build", "Early Access Version"},

            //Main Menu
            { "Resume Game", "Spiel fortsetzen"},
            { "Continue Game", "Spiel fortsetzen"},
            { "Tutorial(Beta)", "Tutorial(Beta)"},
            { "New Game", "Neues Spiel"},
            { "Load Game", "Spiel laden"},
            { "Save Game", "Spiel speichern"},
            { "Achievements", "Errungenschaften"},
            { "Options", "Optionen"},
            { "Exit", "Beenden"},
            //Screens
            //New Game
            { "Town Name:", "Stadtname:"},
            { "Difficulty:", "Schwierigkeitsgrad:"},
            { "Easy", "Leicht"},
            { "Moderate", "Moderat"},
            { "Hard", "Schwer"},
            { "Type of game:", "Art des Spiels:"},
            { "Freewill", "Freier Wille"},
            { "Traditional", "Traditionell"},
            { "New.Game.Pirates", "Piraten (wenn geprüft, könnte die Stadt einen Piratenangriff erleiden)"},
            { "New.Game.Expires", "Lebensmittelverfall (wenn geprüfte Lebensmittel mit der Zeit ablaufen)"},
            { "OK", "OK"},
            { "Cancel", "Abbruch"},
            { "Delete", "Löschen"},
            { "Enter name...", "Namen eingeben..."},
            //Options
            { "General", "Allgemein"},
            { "Unit System:", "Einheitensystem:"},
            { "Metric", "Metrisch"},
            { "Imperial", "Imperial"},
            { "AutoSave Frec:", "Automatisch speichern:"},
            { "20 min", "20 min"},
            { "15 min", "15 min"},
            { "10 min", "10 min"},
            { "5 min", "5 min"},
            { "Language:", "Sprache:"},
            { "English", "Englisch"},
            { "Camera Sensitivity:", "Kamerasensivität:"},
            { "Themes", "Themen"},
            { "Halloween:", "Halloween:"},
            { "Christmas:", "Weihnachten:"},
            { "Options.Change.Theme", "Wenn geändert, lade bitte das Spiel einmal neu"},

            { "Screen", "Bildschirm"},
            { "Quality:", "Qualität:"},
            { "Beautiful", "Schön"},
            { "Fantastic", "Fantastisch"},
            { "Simple", "Einfach"},
            { "Good", "Gut"},
            { "Resolution:", "Auflösung:"},
            { "FullScreen:", "Vollbild:"},

            { "Audio", "Audio"},
            { "Music:", "Musik:"},
            { "Sound:", "Klang:"},
            { "Newborn", "Neugeborenes"},
            { "Build Completed", "Bau abgeschlossen"},
            { "People's Voice", "Volksstimme"},
            
            //in game gui
            { "Prod", "Prod"},
            { "Finance", "Finance"},


            
            //After Oct 20th 2018
            { "Resources", "Resources"},
            { "Dollars", "Dollars"},
            { "Coming.Soon", "This building is coming soon to the game"},
            { "Max.Population", "Can't build more houses. Max population reached"},

            { "To.Unlock", "To unlock: "},
            { "People", "People"},
            { "Of.Food", " of food. "},
            { "Port.Reputation.Least", "Port reputation at least at "},
            { "Pirate.Threat.Less", "Pirate threat less than "},
            { "Skip", "Skip"},

            //After Dec 8, 2018
            { "ReloadMod.HoverSmall", "Reload Mod Files"},
            { "isAboveHeight.MaritimeBound", "Building's land portion is below allowed height"},
            { "arePointsEven.MaritimeBound", "Building's land portion is not in an even terrain"},
            { "isOnTheFloor.MaritimeBound", "Building's land portion is not at the common height"},
            { "isBelowHeight.MaritimeBound", "Building's maritime portion must be in the water"},

            { "InLand.Helper", "On Land"},
            { "InWater.Helper", "On Water"},

            //After Dec 28, 2018
            { "Down.HoverSmall", "Decrease Priority"},
            { "Up.HoverSmall", "Increase Priority"},
            { "Trash.HoverSmall", "Delete Order"},
            { "Counting...", "Counting..."},
            { "Ten Orders Limit", "Ten orders is the limit"},

            //After May 1, 2019
            { "Our inventories:", "Our inventories:"},
            { "Select Product:", "Select Product:"},
            { "Current_Rank.HoverSmall", "Number in Queue"},

            { "Deutsch(Beta)", "Deutsch(Beta)"},



        };

    }

    internal static void Clear()
    {
        _german.Clear();
    }

    internal static Dictionary<string, string> Dictionary()
    {
        return _german;
    }
    
    public static bool ContainsKey(string key)
    {
        return _german.ContainsKey(key);
    }

}