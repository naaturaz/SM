using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class French
{
    private static LangDict _fr = new LangDict();

    /// </summary>
    public static void ReloadDict()
    {
        string _houseTail = ". Les SugarMiller vivent ici et apprécient recevoir un bon repas de temps en temps ";
        string _animalFarmTail = ", vous pouvez élever différents animaux dans ce bâtiment";
        string _fieldFarmTail = ", vous pouvez planter différentes cultures et fruits dans ce bâtiment";
        string _asLongHasInput = ", aussi longtemps qu'il dispose des intrants nécessaires";
        string _produce = "Dans ce bâtiment les ouvriers produiront le produit sélectionné, aussi longtemps qu'il a les entrées nécessaires";
        string _storage =
        "C'est un entrepôt. S'il est plein, les gens ne travailleront plus tant qu'ils n'auront pas d'endroit pour stocker leurs produits";
        string _militar = "Ce bâtiment contribue à réduire la menace pirate sur votre port, pour être efficace il doit y avoir des travailleurs. Plus il y a de travailleurs, mieux c'est";
        string _notRegionNeeded = " Peut être construit sans posséder l'emplacement.";

        _fr = new LangDict();


            //Descriptions
            //Infr
        _fr.Add("Road.Desc","Utilisé à des fins de décoration. Les gens sont plus heureux s'il y a des routes autour d'eux");
        _fr.Add("BridgeTrail.Desc","Permet aux gens de passer d'un côté du terrain à l'autre");
        _fr.Add("BridgeRoad.Desc","Permet aux gens de passer d'un côté du terrain à l'autre. Les gens adorent ces ponts. Il donne un sentiment de prospérité et de bonheur" +_houseTail);
        _fr.Add("LightHouse.Desc","Permet d'augmenter la visibilité du port. Ajoute de la notoriété au port tant qu'il y a des travailleurs");
        _fr.Add( H.Masonry + ".Desc","Bâtiment important, les ouvriers construisent de nouveaux bâtiments et travaillent comme des brouettes une fois qu'ils n'ont rien à faire");
        _fr.Add( H.StandLamp + ".Desc","Illumine la nuit si l'huile de baleine est disponible dans l'entrepôt de la ville");

        _fr.Add( H.HeavyLoad + ".Desc","Ces ouvriers utilisent des chariots attelés pour déplacer des marchandises");


            //House
        _fr.Add("Bohio.Desc", "Maison Bohio, conditions de vie rudimentaires avec des gens malheureux qui peuvent avoir seulement un maximum de 2 à 3 enfants");

        _fr.Add("Shack.Desc", "Cabane, conditions de vie rudimentaires avec des gens malheureux qui peuvent avoir seulement un maximum de 2 enfants");
        _fr.Add("MediumShack.Desc", "Moyenne Cabane, est au-dessus des conditions de vie rudimentaires avec un petit bonheur, peut avoir un maximum de 2 à 3 enfants");
        _fr.Add("LargeShack.Desc", "Grande Cabane, bonnes conditions de vie avec bonheur, peut avoir un maximum de 2 à 4 enfants");


        _fr.Add("WoodHouseA.Desc", "Maison moyenne en bois, une famille peut avoir 2-3 enfants Max" );
        _fr.Add("WoodHouseB.Desc", "Maison moyenne en bois, une famille peut avoir 3-4 enfants Max"  );
        _fr.Add("WoodHouseC.Desc", "Maison moyenne en bois, une famille peut avoir 2-3 enfants Max");
        _fr.Add("BrickHouseA.Desc", "Maison moyenne, une famille peut avoir 3 enfants Max");
        _fr.Add("BrickHouseB.Desc","Grande maison, une famille peut avoir 3-4 enfants Max");
        _fr.Add("BrickHouseC.Desc","Grande maison, une famille peut avoir 4 enfants Max");

            
            //Farms
            //Animal
        _fr.Add("AnimalFarmSmall.Desc","Petite ferme animale"+_animalFarmTail);
        _fr.Add("AnimalFarmMed.Desc","Moyenne ferme animale"+_animalFarmTail);
        _fr.Add("AnimalFarmLarge.Desc","Grande ferme animale"+_animalFarmTail);
        _fr.Add("AnimalFarmXLarge.Desc","Ferme animale extra-large"+_animalFarmTail);
            //Fields
        _fr.Add("FieldFarmSmall.Desc","Petit champ agricole"+_fieldFarmTail);
        _fr.Add("FieldFarmMed.Desc","Moyen champ agricole"+_fieldFarmTail);
        _fr.Add("FieldFarmLarge.Desc","Grand champ agricole"+_fieldFarmTail);
        _fr.Add("FieldFarmXLarge.Desc","Champ agricole extra-large"+_fieldFarmTail);
        _fr.Add( H.FishingHut + ".Desc","Un ouvrier peut pêcher ici des poissons dans la rivière (doit être placée près de la rivière)." + _notRegionNeeded);

            //Raw
        _fr.Add("Mortar.Desc","Un ouvrier produira ici le mortier");
        _fr.Add("Clay.Desc","Un ouvrier produira ici l'argile, la matière première est nécessaire pour des briques et plus");
        _fr.Add("Pottery.Desc","Un ouvrier produira ici des articles de vaisselles, comme les assiettes, les bocaux, etc");
        _fr.Add("Mine.Desc","Un ouvrier peut pêcher ici dans une rivière");
        _fr.Add("MountainMine.Desc","Un ouvrier travaillera ici à la mine en prélevant du minerai");
        _fr.Add("Resin.Desc","Un ouvrier travaillera ici à la mine en prélevant des minerais et des métaux au hasard");
        _fr.Add( H.LumberMill +".Desc","Les ouvriers trouveront ici des ressources telles que le bois, la pierre, et le minerai");
        _fr.Add("BlackSmith.Desc","Les travailleurs produiront ici le produit sélectionné"+_asLongHasInput);
        _fr.Add("ShoreMine.Desc","Les ouvriers produiront ici le sel et le sable");
        _fr.Add("QuickLime.Desc","Les ouvriers produiront ici la chaux vive");

            //Prod
        _fr.Add("Brick.Desc","Un ouvrier produira ici des produits faits d'argile, tels que des briques, etc.");
        _fr.Add("Carpentry.Desc","Un ouvrier produira ici des produits faits de bois, tels que des caisses, des tonneaux, etc.");
        _fr.Add("Cigars.Desc","Les ouvriers produiront ici des cigares"+_asLongHasInput);
        _fr.Add("Mill.Desc","Les ouvriers vont moudre ici des céréales"+_asLongHasInput);
        _fr.Add( H.Tailor+".Desc","Les ouvriers produits ici des vêtements"+_asLongHasInput);
        _fr.Add("Tilery.Desc","Les ouvriers produiront ici des tuiles"+_asLongHasInput);
        _fr.Add("Armory.Desc","Les ouvriers produiront ici des armes"+_asLongHasInput);
        _fr.Add( H.Distillery+".Desc",_produce);
        _fr.Add("Chocolate.Desc",_produce);
        _fr.Add("Ink.Desc",_produce);

            //Ind
        _fr.Add("Cloth.Desc",_produce);
        _fr.Add("GunPowder.Desc",_produce);
        _fr.Add("PaperMill.Desc",_produce);
        _fr.Add("Printer.Desc",_produce);
        _fr.Add("CoinStamp.Desc",_produce);
        _fr.Add("Silk.Desc",_produce);
        _fr.Add("SugarMill.Desc",_produce);
        _fr.Add("Foundry.Desc",_produce);
        _fr.Add("SugarShop.Desc", "Produit des sous-produits du sucre !!!. " + _produce);


            _fr.Add("SteelFoundry.Desc",_produce);

            //trade
        _fr.Add("Dock.Desc","Vous pouvez ajouter ici des commandes d'importation ou d'exportation (doit être placé près de l'océan)." + _notRegionNeeded);
        _fr.Add( H.Shipyard + ".Desc","Vous pouvez réparer vos bâteaux ici, mais vous devez avoir des matériaux de réparation pour bâteaux dans l'inventaire");
        _fr.Add("Supplier.Desc","You can supply ships with goods here, but you must have items in inventory that a ship can use for their long trip");
        _fr.Add("StorageSmall.Desc",_storage);
        _fr.Add("StorageMed.Desc",_storage);
        _fr.Add("StorageBig.Desc",_storage);
        _fr.Add("StorageBigTwoDoors.Desc",_storage);
        _fr.Add("StorageExtraBig.Desc",_storage);

            //gov
        _fr.Add("Library.Desc","Les gens viennent dans ce bâtiment pour lire et emprunter des livres pour leur savoir. Plus les bibliothèques ont de stock, mieux c'est");
        _fr.Add("School.Desc","Les gens vont recevoir ici un enseignement");
        _fr.Add("TradesSchool.Desc","Les gens vont obtenir ici un enseignement spécialisé pour les métiers");
        _fr.Add("TownHouse.Desc","La maison urbaine accroît le bonheur et la prospérité de votre peuple");

            //other
        _fr.Add("Church.Desc","L'église donne du bonheur et de l'espoir à votre peuple");
        _fr.Add("Tavern.Desc","La taverne donne un peu de détente et de divertissement à votre peuple");

            //Militar
        _fr.Add("WoodPost.Desc", "Ils repèrent les bandits et les pirates plus rapidement afin que vous puissiez vous préparer à l'avance");
        _fr.Add("PostGuard.Desc",_militar);
        _fr.Add("Fort.Desc",_militar);
        _fr.Add("Morro.Desc",_militar+". Une fois que vous construisez ceci, les pirates devraient le savoir");

            //Decoration
        _fr.Add("Fountain.Desc", "Sublime votre ville et ajoute du bonheur global à vos citoyens");
        _fr.Add("WideFountain.Desc", "Sublime votre ville et ajoute du bonheur global à vos citoyens");
        _fr.Add("PalmTree.Desc", "Sublime votre ville et ajoute du bonheur global à vos citoyens");
        _fr.Add("FloorFountain.Desc", "Sublime votre ville et ajoute du bonheur global à vos citoyens");
        _fr.Add("FlowerPot.Desc", "Sublime votre ville et ajoute du bonheur global à vos citoyens");
        _fr.Add("PradoLion.Desc", "Sublime votre ville et ajoute du bonheur global à vos citoyens");



            //Buildings name
            //Infr
        _fr.Add("Road","Route");
        _fr.Add("BridgeTrail","Pont de sentier");
        _fr.Add("BridgeRoad","Pont routier");
        _fr.Add("LightHouse","Phare");
        _fr.Add("Masonry","Maçonnerie");
        _fr.Add("StandLamp","Lampadaire");
        _fr.Add("HeavyLoad","Charge lourde");


            //House
        _fr.Add("Shack", "Cabane");
        _fr.Add("MediumShack", "Moyenne cabane");
        _fr.Add("LargeShack", "Grande cabane");

        _fr.Add("WoodHouseA", "Moyenne maison en bois" );
        _fr.Add("WoodHouseB", "Grande maison en bois"  );
        _fr.Add("WoodHouseC", "Maison en bois de luxe");
        _fr.Add("BrickHouseA", "Moyenne maison de brique");
        _fr.Add("BrickHouseB","Maison de brique de luxe");
        _fr.Add("BrickHouseC","Grande maison de brique");

            
            //Farms
            //Animal
        _fr.Add("AnimalFarmSmall","Petite ferme animale");
        _fr.Add("AnimalFarmMed","Moyenne ferme animale");
        _fr.Add("AnimalFarmLarge","Grande ferme animale");
        _fr.Add("AnimalFarmXLarge","Ferme animale extra-large");
            //Fields
        _fr.Add("FieldFarmSmall","Petit champ agricole");
        _fr.Add("FieldFarmMed","Moyen champ agricole");
        _fr.Add("FieldFarmLarge","Grand champ agricole");
        _fr.Add("FieldFarmXLarge","Champ agricole extra-large");
        _fr.Add("FishingHut","Hutte de pêcheur");

            //Raw
        _fr.Add("Mortar","Mortier");
        _fr.Add("Clay","Argile");
        _fr.Add("Pottery","Poterie");
        _fr.Add("MountainMine","Mine de montagne");
        _fr.Add("LumberMill" ,"Scierie");
        _fr.Add("BlackSmith","Forge");
        _fr.Add("ShoreMine","Mine de littoral");
        _fr.Add("QuickLime","Chaux vive");

            //Prod
        _fr.Add("Brick","Brique");
        _fr.Add("Carpentry","Menuiserie");
        _fr.Add("Cigars","Cigares");
        _fr.Add("Mill","Moulin");
        _fr.Add("Tailor","Tailleur");
        _fr.Add("Tilery","Tuilerie");
        _fr.Add("Armory","Armurerie");
        _fr.Add("Distillery","Distillerie");
        _fr.Add("Chocolate","Chocolaterie");
        _fr.Add("Ink","Encre");

            //Ind
        _fr.Add("Cloth","Vêtement");
        _fr.Add("GunPowder","Poudre à canon");
        _fr.Add("PaperMill","Papeterie");
        _fr.Add("Printer","Printer");
        _fr.Add("CoinStamp","Pièce de monnaie");
        _fr.Add("SugarMill","Usine sucrière");
        _fr.Add("Foundry","Fonderie");
        _fr.Add("SteelFoundry","Fonderie d'acier");
        _fr.Add("SugarShop","SugarShop");


            //trade
        _fr.Add("Dock","Port");
        _fr.Add("Shipyard","Chantier naval");
        _fr.Add("Supplier","Fournisseur");
        _fr.Add("StorageSmall","Petit entrepôt");
        _fr.Add("StorageMed","Moyen entrepôt");
        _fr.Add("StorageBig","Grand entrepôt");

            //gov
        _fr.Add("Library","Bibliothèque");
        _fr.Add("School","Ecole");
        _fr.Add("TradesSchool","Ecole de métiers");
        _fr.Add("TownHouse","Maison urbaine");

            //other
        _fr.Add("Church","Eglise");
        _fr.Add("Tavern","Taverne");

            //Militar
        _fr.Add("WoodPost", "Wood Guard Duty");
        _fr.Add("PostGuard","Stone Guard Duty");
        _fr.Add("Fort","Fort");
        _fr.Add("Morro","Morro");

            //Decorations
        _fr.Add("Fountain", "Fontaine");
        _fr.Add("WideFountain", "Grande fontaine");
        _fr.Add("PalmTree", "Palmier");
        _fr.Add("FloorFountain", "Floor Fountain");
        _fr.Add("FlowerPot", "Pot de fleurs");
        _fr.Add("PradoLion", "Lion du Prado");

            //Main GUI
        _fr.Add("SaveGame.Dialog", "Enregistrer votre progression de jeu");
        _fr.Add("LoadGame.Dialog", "Charger une partie");
        _fr.Add("NameToSave", "Enregistrer votre partie en:");
        _fr.Add("NameToLoad", "Partie à charger sélectionnée:");
        _fr.Add("OverWrite", "Il y a déjà une partie enregistrée avec le même nom. Voulez-vous remplacer le fichier?");
        _fr.Add("DeleteDialog", "Êtes-vous sûr de vouloir supprimer la partie enregistrée?");
        _fr.Add("NotHDDSpace", "Pas assez d'espace sur le lecteur {0} pour enregistrer la partie");
        _fr.Add("GameOverPirate", "Désolé, vous avez perdu la partie! Les pirates ont attaqué votre ville et tué tout le monde.");
        _fr.Add("GameOverMoney", "Désolé, vous avez perdu la partie! La Couronne ne soutiendra plus votre île des Caraïbes.");
        _fr.Add("BuyRegion.WithMoney", "Êtes-vous sûr de vouloir acheter cet emplacement?.");
        _fr.Add("BuyRegion.WithOutMoney", "Désolé, vous ne pouvez pas vous le permettre maintenant.");
        _fr.Add("Feedback", "Un avis!? Super...:) Merci. 8) ");
        _fr.Add("OptionalFeedback", "Les avis sont importants. SVP laissez un commentaire ");
        _fr.Add("MandatoryFeedback", "Seulement l'équipe des développeurs verront ceci. Votre note est?");
        _fr.Add("PathToSeaExplain", "Affiche le chemin le plus court vers la mer.");


        _fr.Add("BugReport", "Vous avez trouvé un bug? uhmm miam-miam.... Envoyez-le nous!! Merci");
        _fr.Add("Invitation", "L'email de votre ami pour avoir une chance de rejoindre la Beta privée");
        _fr.Add("Info", "");//use for informational Dialogs
        _fr.Add("Negative", "La Couronne vous a accordé une marge de crédit. Si vous possédez plus de $100 000,00 c'est Game Over");  


            //MainMenu
            _fr.Add("Types_Explain", "Traditional: \nC'est un jeu où, au début, certains bâtiments sont verrouillés et vous devez les déverrouiller. " +
                    "La bonne nouvelle c'est que vous aurez des conseils." +
                    "\n\nFreewill: \nTous les bâtiments disponibles sont déverouillés tout de suite. " +
                    "La mauvaise nouvelle c'est que de cette façon vous pouvez échouer très facilement." +
                    "\n\nLa difficulté 'Difficile' est la plus proche de la réalité");


            //Tooltips
            //Small Tooltips
        _fr.Add("Person.HoverSmall", "Total/Adultes/Enfants");
        _fr.Add("Emigrate.HoverSmall", "Emigré");
        _fr.Add("CurrSpeed.HoverSmall", "Vitesse de jeu");
        _fr.Add("Town.HoverSmall", "Nom de la ville");
        _fr.Add("Lazy.HoverSmall", "Sans-emploi");
        _fr.Add("Food.HoverSmall", "Nourriture");
        _fr.Add("Happy.HoverSmall", "Bonheur");
        _fr.Add("PortReputation.HoverSmall", "Notoriété du port");
        _fr.Add("Dollars.HoverSmall", "Dollars");
        _fr.Add("PirateThreat.HoverSmall", "Menace pirate");
        _fr.Add("Date.HoverSmall", "Date (Mmm/A)");
        _fr.Add("MoreSpeed.HoverSmall", "Augmenter [PgUp]");
        _fr.Add("LessSpeed.HoverSmall", "Diminuer [PgDwn]");
        _fr.Add("PauseSpeed.HoverSmall", "Mettre en pause");
        _fr.Add("CurrSpeedBack.HoverSmall", "Vitesse actuelle");
        _fr.Add("ShowNoti.HoverSmall", "Notifications");
        _fr.Add("Menu.HoverSmall", "Menu principal");
        _fr.Add("QuickSave.HoverSmall", "Sauvegarde rapide[Ctrl+S][F]");
        _fr.Add("Bug Report.HoverSmall", "Signaler un bug");
        _fr.Add("Feedback.HoverSmall", "Commentaire");
        _fr.Add("Hide.HoverSmall", "Cacher");
        _fr.Add("CleanAll.HoverSmall", "Nettoyer");
        _fr.Add("Bulletin.HoverSmall", "Contrôle/Bulletin");
        _fr.Add("ShowAgainTuto.HoverSmall","Didacticiel");
        _fr.Add("BuyRegion.HoverSmall", "Acheter des emplacements");
        _fr.Add("Help.HoverSmall", "Aider");

        _fr.Add("More.HoverSmall", "Plus");
        _fr.Add("Less.HoverSmall", "Moins");
        _fr.Add("", "Précédent");

        _fr.Add("More Positions.HoverSmall", "Plus");
        _fr.Add("Less Positions.HoverSmall", "Moins");


            //down bar
        _fr.Add("Infrastructure.HoverSmall", "Infrastructure");
        _fr.Add("House.HoverSmall", "Logement");
        _fr.Add("Farming.HoverSmall", "Agriculture");
        _fr.Add("Raw.HoverSmall", "Matière première");
        _fr.Add("Prod.HoverSmall", "Production");
        _fr.Add("Ind.HoverSmall", "Industrie");
        _fr.Add("Trade.HoverSmall", "Commerce");
        _fr.Add("Gov.HoverSmall", "Gouvernement");
        _fr.Add("Other.HoverSmall", "Autre");
        _fr.Add("Militar.HoverSmall", "Militaire");
        _fr.Add("Decoration.HoverSmall", "Décoration");

        _fr.Add("WhereIsTown.HoverSmall", "Retourner à la ville [P]");
        _fr.Add("WhereIsSea.HoverSmall", "Afficher Ocean Path");
        _fr.Add("Helper.HoverSmall", "Aide");
        _fr.Add("Tempeture.HoverSmall", "Température");
            
            //building window
        _fr.Add("Gen_Btn.HoverSmall", "Onglet Général");
        _fr.Add("Inv_Btn.HoverSmall", "Onglet Inventaire");
        _fr.Add("Upg_Btn.HoverSmall", "Onglet Améliorations");
        _fr.Add("Prd_Btn.HoverSmall", "Onglet Production");
        _fr.Add("Sta_Btn.HoverSmall", "Onglet Statistiques");
        _fr.Add("Ord_Btn.HoverSmall", "Onglet Commandes");
        _fr.Add("Stop_Production.HoverSmall", "Arrêter la production");
        _fr.Add("Demolish_Btn.HoverSmall", "Démolir");
        _fr.Add("More Salary.HoverSmall", "Payer plus");
        _fr.Add("Less Salary.HoverSmall", "Payer moins");
        _fr.Add("Next_Stage_Btn.HoverSmall", "Acheter le prochain niveau");
        _fr.Add("Current_Salary.HoverSmall", "Salaire actuel");
        _fr.Add("Current_Positions.HoverSmall", "Positions actuelles");
        _fr.Add("Max_Positions.HoverSmall", "Positions max");


        _fr.Add("Add_Import_Btn.HoverSmall", "Ajouter une importation");
        _fr.Add("Add_Export_Btn.HoverSmall", "Ajouter une exportation");
        _fr.Add("Upg_Cap_Btn.HoverSmall", "Améliorer les capacités");
        _fr.Add("Close_Btn.HoverSmall", "Fermer");
        _fr.Add("ShowPath.HoverSmall", "Montrer le chemin");
        _fr.Add("ShowLocation.HoverSmall", "Afficher l'emplacement");//TownTitle
        _fr.Add("TownTitle.HoverSmall", "Ville");
        _fr.Add("WarMode.HoverSmall", "Mode Combat");
        _fr.Add("BullDozer.HoverSmall", "Bulldozer");
        _fr.Add("Rate.HoverSmall", "Rate Me");

            //addOrder windiw
        _fr.Add("Amt_Tip.HoverSmall", "Quantité de produit");

            //Med Tooltips 
        _fr.Add("Build.HoverMed", "Placer un bâtiment: 'Clic Gauche' \n" +
                                "Pivoter un bâtiment: Touche 'R' \n" +
                                "Annuler: 'Clic Droit'");
            _fr.Add("BullDozer.HoverMed", "Nettoyer une zone: 'Clic Gauche' \n" + 
                "Annuler: 'Clic Droit' \nCoût: $10 par utilisation ");

            _fr.Add("Road.HoverMed", "Commencer: 'Clic Gauche' \n" +
                    "Agrandir: 'Bouger la souris' \n" +
                    "Fixer: 'Clic Gauche encore' \n" +
                "Annuler: 'Clic Droit'");

        _fr.Add("Current_Salary.HoverMed", "Les travailleurs iront au travail, où le salaire est le plus élevé." +
                                            " Si 2 places offrent le même salaire, alors l'endroit le plus proche de la maison sera choisi.");



            //Notifications
        _fr.Add("BabyBorn.Noti.Name", "Nouveau-né");
        _fr.Add("BabyBorn.Noti.Desc", "{0} est né");
        _fr.Add("PirateUp.Noti.Name", "Les Pirates sont proche");
        _fr.Add("PirateUp.Noti.Desc", "Les Pirates sont proche du rivage");
        _fr.Add("PirateDown.Noti.Name", "Les Pirates vous respectent");
        _fr.Add("PirateDown.Noti.Desc", "Les Pirates vous respectent un peu plus aujourd'hui");

        _fr.Add("Emigrate.Noti.Name", "Un citoyen a émigré");
        _fr.Add("Emigrate.Noti.Desc", "Les gens émigrent quand ils ne sont pas satisfaits de votre gouvernement");
        _fr.Add("PortUp.Noti.Name", "Le port est connu");
        _fr.Add("PortUp.Noti.Desc", "Votre notoriété portuaire est en hausse avec les ports voisins et les routes");
        _fr.Add("PortDown.Noti.Name", "Le port est moins connu");
        _fr.Add("PortDown.Noti.Desc", "Votre notoriété portuaire a baissée");

        _fr.Add("BoughtLand.Noti.Name", "Nouveau terrain acheté");
        _fr.Add("BoughtLand.Noti.Desc", "Une nouvelle zone foncière a été achetée");

        _fr.Add("ShipPayed.Noti.Name", "Bâteau payé");
        _fr.Add("ShipPayed.Noti.Desc", "Un bâteau a payé {0} pour des biens ou des services");
        _fr.Add("ShipArrived.Noti.Name", "Un bâteau est arrivé");
        _fr.Add("ShipArrived.Noti.Desc", "Un nouveau bâteau est arrivé à l'un de nos bâtiments maritimes");

        _fr.Add("AgeMajor.Noti.Name", "Nouveau travailleur");
        _fr.Add("AgeMajor.Noti.Desc", "{0} est prêt à travailler");


        _fr.Add("PersonDie.Noti.Name", "Une personne est décédée");
        _fr.Add("PersonDie.Noti.Desc", "{0} est mort");

        _fr.Add("DieReplacementFound.Noti.Name", "Une personne est décédée");
        _fr.Add("DieReplacementFound.Noti.Desc", "{0} est mort. Un remplaçant pour l'emploi a été trouvé.");

        _fr.Add("DieReplacementNotFound.Noti.Name", "Une personne est décédée");
        _fr.Add("DieReplacementNotFound.Noti.Desc", "{0} est mort. Aucun remplaçant pour le poste a été trouvé");


        _fr.Add("FullStore.Noti.Name", "Un entrepôt déborde");
        _fr.Add("FullStore.Noti.Desc", "Un entrepôt est à {0}% capacités");

        _fr.Add("CantProduceBzFullStore.Noti.Name", "Un ouvrier ne peut pas produire");
        _fr.Add("CantProduceBzFullStore.Noti.Desc", "{0} parce que l'entrepôt cible est complet");

        _fr.Add("NoInput.Noti.Name", "Au moins une entrée est manquante dans la construction");
        _fr.Add("NoInput.Noti.Desc", "Un bâtiment ne peut pas produire {0} car il manque au moins une entrée");

        _fr.Add("Built.Noti.Name", "Un bâtiment a été construit");
        _fr.Add("Built.Noti.Desc", "{0} a été entièrement construit");

        _fr.Add("cannot produce", "ne peut pas produire");

            



            //Main notificaion
            //Shows on the middle of the screen
        _fr.Add("NotScaledOnFloor", "Le bâtiment est soit trop près de la rive ou de la montagne");
        _fr.Add("NotEven", "Le sol sous le bâtiment n'est même pas");
        _fr.Add("Colliding", "Le bâtiment est en collision avec un autre");
        _fr.Add("Colliding.BullDozer", "Le bulldozer est en collision avec un bâtiment. Ne peut être utilisé sur le terrain (arbres, roches)");

        _fr.Add("BadWaterHeight", "Le bâtiment est trop bas ou haut sur l'eau");
        _fr.Add("LockedRegion", "Vous devez posséder cette zone pour construire ici");
        _fr.Add("HomeLess", "Les gens de cette maison n'ont nulle part où aller. SVP construiser une nouvelle maison" +
                            " peut contenir cette famille et essayer à nouveau");
        _fr.Add("LastFood", "Impossible de détruire, c'est le seul entrepôt dans votre village");
        _fr.Add("LastMasonry", "Impossible de détruire, c'est la seule maçonnerie dans votre village");
        _fr.Add("OnlyOneDemolish", "Vous démolissez déjà un bâtiment. Essayer à nouveau après que la démolition soit terminée");


            //help

        _fr.Add("CMed", "Pour la construction d'un bâtiment, vous devez avoir des ouvriers dans la maçonnerie. "+
                    " Cliquez sur la maçonnerie puis sur le signe '+' dans l'onglet général. Assurez-vous d'avoir suffisamment de ressources");
        _fr.Add("Demolition.HoverMed", "Once the inventory is clear will be demolished. Les brouettes déplaceront le stock");

        _fr.Add("Construction.Help", "Pour la construction d'un bâtiment, vous devez avoir des ouvriers dans la maçonnerie. "+
                    " Cliquez sur la maçonnerie, puis sur le signe '+' dans l'onglet général. Assurez-vous d'avoir suffisamment de ressources");
        _fr.Add("Camera.Help", "Camera: Utiliser [WASD] ou le curseur pour la déplacer. " +
                        "Utilisez la molette de défilement de votre souris, maintenez-la enfoncée pour faire pivoter, ou [Q] et [E]");
        _fr.Add("Sea Path.Help", "Cliquez sur le coin inférieur gauche 'Afficher/masquer le chemin de la mer' " +
                            "bouton pour afficher le chemin le plus proche de la mer");

        _fr.Add("People Range.Help", "L'énorme cercle bleu autour de chaque bâtiment désigne sa portée");

        _fr.Add("Pirate Threat.Help", "Menace Pirate: C'est ainsi que les Pirates de votre port sont informés. Cela augmente comme " +
                                        " Vous avez plus d'argent. Si cela atteint plus de 90, vous perdrez la partie. Vous pouvez contrer la menace en construisant des bâtiments militaires");

        _fr.Add("Port Reputation.Help", "Notoriété du port: Plus les gens connaissent votre port, plus ils vont le visiter." +
                                            " Si vous voulez l'augmenter, assurez vous que vous avez toujours des commandes" +
                                            " dans le port");
        _fr.Add("Emigrate.Help", "Emigré: Quand les gens sont malheureux pendant quelques années, ils partent. Le pire" +
                                    " c'est qu'ils ne reviendront pas, ils ne produiront rien ou n'auront pas d'enfants." +
                                    " La bonne nouvelle c'est qu'ils augmentent la 'notoriété du port'");
        _fr.Add("Food.Help", "Nourriture: Plus la variété d'aliments disponible dans un ménage est grande, plus ils seront" +
                                " heureux.");

        _fr.Add("Weight.Help", "Poids: Tous les poids dans le jeu sont en kg ou lbs selon le système d'unité qui est sélectionné." +
                                " Vous pouvez le changer dans les options dans le menu principal'");
        _fr.Add("What is Ft3 and M3?.Help", "La capacité de stockage est déterminée par le volume du bâtiment. Ft3 c'est un cubic foot. M3 c'est un mètre cube" );//. Keep in mind that less dense products will fill up your storage quickly. To see products density Bulletin/Prod/Spec");

        _fr.Add("More.Help", "Si vous avez besoin d'aides supplémentaires, il serait peut être utile de terminer le didacticiel, ou tout simplement poster une question sur les forums de SugarMill's");

                //more 
        _fr.Add("Products Expiration.Help", "Péremption des produits: Tout comme dans la vie réelle, dans ce jeu, chaque produit se périme. Certains produits alimentaires se périment plus tôt que d'autres. Vous pouvez voir combien de produits ont périmés dans le Bulletin/Prod/Péremption");
        _fr.Add("Horse Carriages.Help", "Comme le jeu a des mesures réelles les gens ne peuvent pas transporter de trop. C'est alors que les chariots tirés par des chevaux entrent en place. Ils transportent beaucoup plus, en conséquence, votre économie est boostée. Une personne dans ses meilleures années pourrait porter environ 15 kg, une brouette proche des 60KG, mais le plus petit chariot peut transporter 240KG. Pour les utiliser, construisez un HeavyLoad ");
        _fr.Add("Usage of goods.Help", "Utilisation des marchandises: Les caisses, les tonneaux, les brouettes, les chariots, les outils, les vêtements, la vaisselle, les meubles et les ustensiles sont tous nécessaires pour faire les activités traditionnelles d'une ville. Comme ces marchandises s'usent, elles diminuent, par conséquent, une personne ne portera rien s'il n'y a pas de caisses. Gardez y un œil ;)");
        _fr.Add("Happiness.Help", "Bonheur: Le bonheur des gens est influencé par différents facteurs. Le nombre d'argent qu'ils possèdent, la variété de nourriture, la satisfaction de la religion, l'accès aux loisirs, le confort de la maison et le niveau d'éducation. Aussi, si une personne a accès à des ustensiles, la vaisselle et les vêtements influenceront leur bonheur.");
        _fr.Add("Line production.Help", "Production à la chaîne: Pour fabriquer un simple clou, vous avez besoin de minerai de la mine, dans la fonderie fondre du fer, et enfin à la forge fabriquer le clou. Si vous avez assez d'argent, vous pouvez toujours acheter le clou directement sur un bâteau, ou tout autre produit.");
        _fr.Add("Bulletin.Help", "L'icône de pages de la barre inférieure est la fenêtre Bulletin/contrôle. Prenez une minute pour l'explorer.");
        _fr.Add("Trading.Help", "Vous devez avoir au moins un port pour faire du commercer. Au port, vous pouvez ajouter des commandes d'importation/exportation et faire de l'argent comptant. Si vous avez besoin d'aide pour ajouter une commande, il serait peut-être utile de compléter le didacticiel");

        _fr.Add("Combat Mode.Help", "Il s'active quand un Pirate/Bandit est détecté par l'un de vos citoyens. Une fois que le mode est actif, vous pouvez commander des unités directement à l'attaque. Sélectionnez-les et cliquez avec le bouton droit pour cibler pour attaquer");

        _fr.Add("Population.Help", "Une fois qu'ils ont 16 ans, ils se déplaceront dans une maison libre s'ils en trouvent. S'il y a toujours une maison libre pour augmenter la croissance, la population sera garantie. S'ils entrent dans les nouvelles maisons à 16 ans, vous Maximisez la croissance de la population");


        _fr.Add("F1.Help", "Appuyez sur [F1] pour l'Aide");

        _fr.Add("Inputs.Help", "Si un bâtiment ne peut pas produire parce qu'il manque des entrées. Vérifiez que vous avez l'entrée (s) nécessaire (s) dans l'entrepôt principal et au moins un ouvrier en maçonnerie");
        _fr.Add("WheelBarrows.Help", "Les brouettes sont les ouvriers de maçonnerie. S'ils n'ont rien à construire, ils agiront comme des brouettes. Si vous avez besoin d'entrées pour entrer dans un bâtiment spécifique Assurez-vous d'en avoir assez qui travaillent et vérifiez aussi les entrées mentionnées dans le bâtiment de stockage");

        _fr.Add("Production Tab.Help", "Si le bâtiment est un champ agricole, assurez-vous que vous avez des travailleurs à la ferme. La récolte sera perdue si elle se tient un mois après le jour de la récolte");
        _fr.Add("Our Inventories.Help", "La section 'Nos stocks' dans la fenêtre 'Ajouter une Commande' est un résumé de ce que nous avons obtenu dans nos stocks de bâtiments de stockage");
        _fr.Add("Inventories Explanation.Help", "Voici un résumé de ce que nous avons obtenu dans nos stocks. Les articles dans d'autres bâtiments de stockage n'appartiennent pas à la ville");

            ///word and grammarly below




            //to  add on spanish         //to correct  
        _fr.Add("TutoOver", "Votre récompense est de $10 000,00 si c'est la première fois que vous le terminez. Le didacticiel est terminé maintenant vous pouvez continuer à jouer à cette partie ou en commencer une nouvelle.");

            //Tuto
        _fr.Add("CamMov.Tuto", "La récompense de fin du didacticiel est de $10 000 (une seule récompense par partie). Etape 1: utilisez [WASD] ou les touches fléchées pour déplacer la caméra. Faites-le pendant au moins 5 secondes");
        _fr.Add("CamMov5x.Tuto", "Utilisez [WASD] ou les touches fléchées et maintenez la touche Shift gauche enfoncée pour déplacer la caméra 5 fois plus vite. Faites-le pendant au moins 5 secondes");
        _fr.Add("CamRot.Tuto", "Maintenant, appuyez sur la molette de défilement vers le bas sur votre souris et déplacez votre souris pour faire pivoter la caméra. Faites-le pendant au moins 5 secondes");


        _fr.Add("BackToTown.Tuto", "Appuyez sur la touche [P] du clavier pour accéder à la position initiale de la caméra.");

        _fr.Add("BuyRegion.Tuto", "ZONES: Vous devez posséder une zone pour pouvoir y construire. Cliquez sur le signe '+' sur la barre inférieure, puis sur le signe 'A Vendre' dans le" + 
                    " milieu d'une zone pour l'acheter. Certains bâtiments peuvent être construits sans posséder la zone (Hutte de pêcheur, Port," + 
                    " Mine de montagne, Mine du littoral, Lampadaire, Poste de guarde)"
                    );

        _fr.Add("Trade.Tuto", "C'était facile, la partie la plus difficile est à venir. Cliquez sur le bouton 'Commerce', situé dans la barre inférieure droite. "+ 
                "Survolez-le, un menu contextuel 'Commerce' apparaîtra");
        _fr.Add("CamHeaven.Tuto", "Faites un défilement arrière avec le bouton central de la souris jusqu'à ce que la caméra atteigne"
                    + " le ciel. Cette vue est utile pour placer de plus grands bâtiments tels que le port");

        _fr.Add("Dock.Tuto", "Maintenant, cliquez sur le 'Port', c'est le premier bouton. Lorsque vous le survolez, il affichera"+
                " sont coût et sa description");
        _fr.Add("Dock.Placed.Tuto", "Maintenant, plus difficile, lisez attentivement. Notez que vous pouvez utiliser la "+
                " touche 'R' pour faire pivoter, et faites un clic droit pour annuler le bâtiment. Ce bâtiment a une partie dans l'océan et une autre sur la terre." +
                " La flèche va en direction de la mer, la zone de stockage va sur terre. Une fois que la flèche est de couleur blanche, clic gauche.");
        _fr.Add("2XSpeed.Tuto", "Augmentez la vitesse du jeu, accédez au contrôleur de vitesse de simulation d'écran du milieu supérieur, cliquez sur le "
                    +" bouton 'Plus de Vitesse' 1 fois jusqu'à ce que 2x soit affiché");

        _fr.Add("ShowWorkersControl.Tuto", "Cliquez sur le bouton 'Contrôle/Bulletin', situé dans la barre inférieure. "+ 
                "Survolez-le, un menu contextuel 'Contrôle/Bulletin' apparaîtra");
        _fr.Add("AddWorkers.Tuto", "Cliquez sur le signe' + 'à droite du bâtiment de maçonnerie, c'est ainsi que vous ajoutez plus de travailleurs.");
        _fr.Add("HideBulletin.Tuto", "Gardez à l'esprit que dans cette fenêtre vous pouvez contrôler et voir différents aspects du jeu. Cliquez à l'extérieur de la fenêtre pour la fermer ou sur le bouton 'OK'.");
        _fr.Add("FinishDock.Tuto", "Terminez maintenant la construction du port. Plus il y a de travailleurs en maçonnerie, plus cela sera rapide."
            + "Assurez-vous également que vous avez tous les matériaux nécessaires pour le construire");
        _fr.Add("ShowHelp.Tuto", "Cliquez sur le bouton 'Aide', situé dans la barre inférieure. "+
                "Survolez-le, un menu contextuel 'Aide' apparaîtra. Là, vous y trouverez quelques conseils.");


        _fr.Add("SelectDock.Tuto", "Les bâteaux déposent et ramassent des marchandises au hasard via les stocks du port. Les ouvriers sont nécessaires pour déplacer des marchandises d'arrimage dedans et dehors. Ils ont besoin de brouettes et de caisses. Si aucun élément ne se trouve dans le stockage du port, ils ne fonctionneront pas. Maintenant, cliquez sur le port.");


        _fr.Add("OrderTab.Tuto", "Accédez à l'onglet commandes de la fenêtre du port.");
        _fr.Add("ImportOrder.Tuto", "Cliquez sur le signe '+' à côté du bouton Ajouter une Commande d'Importation.");

        _fr.Add("AddOrder.Tuto", "Faites défiler les produits vers le bas, sélectionnez le bois et entrez 100 comme montant. Cliquez ensuite sur le bouton 'ajouter'.");
        _fr.Add("CloseDockWindow.Tuto", "Maintenant, la commande est ajoutée. Un bâteau aléatoire déposera cet élément dans le stockage du port. Ensuite, vos ouvriers du port l'emmèneront à l'entrepôt le plus proche. Maintenant, cliquez hors de la fenêtre, de sorte qu'elle se ferme.");
        _fr.Add("Rename.Tuto", "Cliquez sur une personne, puis cliquez sur la barre de titre de la personne. Vous pouvez changer le nom de n'importe quelle personne ou bâtiment dans le jeu. Cliquez à l'extérieur pour que la modification soit sauvegardée ");
        _fr.Add("RenameBuild.Tuto", "Cliquez maintenant sur un bâtiment et changez son nom de la même manière. N'oubliez pas de cliquer à l'extérieur de sorte que le changement soit enregistré");

        _fr.Add("BullDozer.Tuto", "Cliquez maintenant sur l'icône bulldozer dans la barre inférieure. Retirez un arbre ou une roche du relief.");


        _fr.Add("Budget.Tuto", "Cliquez sur le bouton 'Contrôle/Bulletin', puis sur le menu 'Finance', puis sur 'Registre'. Il s'agit du grand-livre du jeu");
        _fr.Add("Prod.Tuto", "Cliquez sur le menu 'Prod' puis sur 'produire'. Cela montrera la production du village pour les 5 dernières années");
        _fr.Add("Spec.Tuto", "Cliquez sur le menu 'Prod', puis sur 'Spec'. Ici vous pouvez voir exactement comment fabriquer chaque produit sur le jeu. Les intrants nécessaires et où cela est produit. Aussi, les prix à l'importation et à l'exportation ");
        _fr.Add("Exports.Tuto", "Cliquez sur le menu 'Finances' puis sur 'Exporter'. Ici vous pouvez voir une répartition des exportations de votre village ");


                //Quest
        _fr.Add("Tutorial.Quest", "Quête: Terminer le didacticiel. Récompense de $10 000. Il faut environ 3 minutes pour le terminer");

        _fr.Add("Lamp.Quest", "Quête: Construire un lampadaire. Trouvez-le dans l'infrastructure, il brille la nuit si il ya de l'huile de baleine dans l'entrepôt");

        _fr.Add("Shack.Quest", "Quête: Construire une cabane. Ce sont des maisons bon marché. Quand les gens ont 16a, ils se déplacent vers une maison de libre si ils en trouvent. De cette façon, la croissance de la population sera garantie. [F1] Aide. Si vous voyez de la fumée dans la cheminée d'une maison, cela signifie qu'il ya des gens qui y vivent ");

        _fr.Add("SmallFarm.Quest", "Quête: Construire une petite ferme agricole. Vous avez besoin de fermes pour nourrir votre peuple");
        _fr.Add("FarmHire.Quest", "Quête: Engager deux fermiers dans la petite ferme agricole. Cliquez sur la ferme et dans le signe plus affecter les travailleurs. Vous devez avoir des gens"
                    +" sans emploi pour pouvoir les assigner dans un nouveau bâtiment");



        _fr.Add("FarmProduce.Quest", "Quête: Produisons " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " des haricots dans la petite ferme agricole. Cliquez sur l'onglet 'Stat' et cela vous montrera la production des 5 dernières années. Vous pouvez voir le progrès de la quête dans la fenêtre de quête. Si vous construisez plus de petites fermes elles seront comptabilisés pour la quête");
        _fr.Add("Transport.Quest", "Quête: Transporter les haricots de la ferme jusqu'à l'entrepôt. Pour ce faire, assurez-vous qu'" +
                " ils y a des ouvriers en maçonnerie. Ils agissent comme des brouettes quand ils ne construisent pas");


        _fr.Add("HireDocker.Quest", "Quête: Embaucher un docker. La seule tâche des dockers est de déplacer les marchandises dans le port via l'entrepôt si vous exportez."+
            " Ou vice-versa en cas d'importation. Ils travaillent lorsqu'il y a un ordre en place et que les marchandises sont prêtes à être transportées. Sinon, ils restent à la maison au repos." +
                " Si vous n'avez pas construit de port, construisez-en un."+
            " Rechercher le dans le Commerce." );


        _fr.Add("Export.Quest", "Quête: Au port, créez une commande et exportez exactement 300 " + Unit.CurrentWeightUnitsString() + " d'haricots."+
                " Au port, cliquez sur l'onglet 'Commandes' et ajoutez une commande d'exportation avec le signe '+'."+
            " Sélectionnez le produit et entrez le montant");



        _fr.Add("MakeBucks.Quest", "Quête: Avoir $100 en exporant des marchandises au port. "+
            "Une fois qu'un bâteau arrive, il paiera de façon aléatoire le produit(s) dans le stockage de votre port");
        _fr.Add("HeavyLoad.Quest", "Quête: Construire un bâtiment HeavyLoad. Ce sont des transporteurs qui portent plus de poids. Ils seront utiles lors du transport des marchandises." ); //Carts must be available on towns storages for them to work");
        _fr.Add("HireHeavy.Quest", "Quête: Dans le bâtiment HeavyLoad, embaucher un transporteur lourd.");


        _fr.Add("ImportOil.Quest", "Quête: Importer 500 " + Unit.CurrentWeightUnitsString() + " d'huiles de baleine au port. Il est nécessaire de garder vos lumières allumées la nuit. Les bâteaux livreront les importations au hasard dans le stockage du port");

        _fr.Add("Population50.Quest", "Atteindre une population totale de 50 citoyens");

            //added Aug 11 2017, result: sep 9(30% off biggest sale ever)
        _fr.Add("Production.Quest", "Nous allons produire des armes maintenant et les vendre plus tard. Tout d'abord, construisez une forge. Trouvez-la dans le menu 'Matières Premières' des bâtiments");
        _fr.Add("ChangeProductToWeapon.Quest", "Dans l'onglet 'produits' de la forge, changez la production en arme. Les ouvriers apporteront les matières premières nécessaires pour forger des armes s'ils en trouvent");
        _fr.Add("BlackSmithHire.Quest", "Embaucher 2 forgerons");
        _fr.Add("WeaponsProduce.Quest", "Produisons " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " d'armes à la forge. Cliquez sur l'onglet 'Stat' et cela vous montrera la production des 5 dernières années. Vous pouvez voir la progression de la quête dans la fenêtre Quête.");
        _fr.Add("ExportWeapons.Quest", "Exportons 100 " + Unit.CurrentWeightUnitsString() + " d'armes. Au port, ajoutez une commande d'exportation. Notez que la vente d'arme est une entreprise rentable.");


        _fr.Add("CompleteQuest", "Votre récompense est {0}");


            //added Sep 14 2017
        _fr.Add("BuildFishingHut.Quest", "Construisez une butte de pêcheur. De cette façon, les citoyens ont des aliments différents à manger, ce qui se traduit par du bonheur");
        _fr.Add("HireFisher.Quest", "Embaucher un pêcheur");

        _fr.Add("BuildLumber.Quest", "Construisez une scierie. Trouvez-la dans le menu 'Matières Premières' des bâtiments");
        _fr.Add("HireLumberJack.Quest", "Embaucher un bucheron");

        _fr.Add("BuildGunPowder.Quest", "Fabriquez de la poudre à canon. Trouvez-la dans le menu des bâtiments de l'industrie");
        _fr.Add("ImportSulfur.Quest", "Au port, importer 1000 " + Unit.CurrentWeightUnitsString() + " de souffre");
        _fr.Add("GunPowderHire.Quest", "Embaucher un ouvrier dans le bâtiment de poudre à canon");

        _fr.Add("ImportPotassium.Quest", "Au port, importer 1000 " + Unit.CurrentWeightUnitsString() + " de potassium");
        _fr.Add("ImportCoal.Quest", "Au port, importer 1000 " + Unit.CurrentWeightUnitsString() + " de charbon");

        _fr.Add("ProduceGunPowder.Quest", "Produisons " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " de poudre à canon. Notez que vous aurez besoin de soufre, de potassium et de charbon pour produire de la poudre à canon");
        _fr.Add("ExportGunPowder.Quest", "Au port exporter 100 " + Unit.CurrentWeightUnitsString() + " de poudre à canon");

        _fr.Add("BuildLargeShack.Quest", "Construire une Grande Cabane et la population se développera plus rapidement");

        _fr.Add("BuildA2ndDock.Quest", "Construire un second port. Ce port ne peut être utilisé que pour les importations, vous pouvez importer des matières premières ici et les exporter à un autre port");
        _fr.Add("Rename2ndDock.Quest", "Renommez les ports, afin qu'ils ne soient utilisés que pour les importations et les exportations");

        _fr.Add("Import2000Wood.Quest", "Au port d'importations, importer 2000 " + Unit.CurrentWeightUnitsString() + " de bois. Cette matière première est nécessaire pour tout, car elle est utilisée comme combustible");

            //IT HAS FINAL MESSAGE 
            //last quest it has a final message to the player. if new quest added please put the final message in the last quest
        _fr.Add("Import2000Coal.Quest", "Au port d'importations, importer 2000 " + Unit.CurrentWeightUnitsString() + " de charbon. Le charbon aussi, est nécessaire pour tout parce qu'il est utilisé comme combustible. J'espère que vous avez apprécié l'expérience jusqu'ici. Continuez à agrandir votre colonie, et sa richesse. Aidez-nous à améliorer le jeu. Participez aux forums en ligne votre voix et vos opinions sont importantes! Amusez-vous Sugarmiller!");

            //



            //Quest Titles
        _fr.Add("Tutorial.Quest.Title", "Didacticiel");
        _fr.Add("Lamp.Quest.Title", "Lampadaire");

        _fr.Add("Shack.Quest.Title", "Construire une cabane");
        _fr.Add("SmallFarm.Quest.Title", "Construire une ferme agricole");
        _fr.Add("FarmHire.Quest.Title", "Embaucher 2 fermiers");


        _fr.Add("FarmProduce.Quest.Title", "Producteur agricole");

        _fr.Add("Export.Quest.Title", "Exportations");
        _fr.Add("HireDocker.Quest.Title", "Embaucher un docker");
        _fr.Add("MakeBucks.Quest.Title", "Gagner de l'argent");
        _fr.Add("HeavyLoad.Quest.Title", "Heavy Load");
        _fr.Add("HireHeavy.Quest.Title", "Embaucher un transporteur lourd");

        _fr.Add("ImportOil.Quest.Title", "Huile de baleine");

        _fr.Add("Population50.Quest.Title", "50 citoyens");
            
            //
        _fr.Add("Production.Quest.Title", "Fabriquer des armes");
        _fr.Add("ChangeProductToWeapon.Quest.Title", "Changer le produit");
        _fr.Add("BlackSmithHire.Quest.Title", "Embaucher 2 forgerons");
        _fr.Add("WeaponsProduce.Quest.Title", "Forger les armes");
        _fr.Add("ExportWeapons.Quest.Title", "Faire des bénéfices" );
            
            //
        _fr.Add("BuildFishingHut.Quest.Title", "Construire une hutte de pêcheur");
        _fr.Add("HireFisher.Quest.Title", "Embaucher un pêcher");
        _fr.Add("BuildLumber.Quest.Title", "Construire une scierie");
        _fr.Add("HireLumberJack.Quest.Title", "Embaucher un bucheron");
        _fr.Add("BuildGunPowder.Quest.Title", "Fabriquer de la poudre à canon");
        _fr.Add("ImportSulfur.Quest.Title", "Importer du souffre");
        _fr.Add("GunPowderHire.Quest.Title", "Embaucher un travailleur de poudre à canon");
        _fr.Add("ImportPotassium.Quest.Title", "Importer du potassium");
        _fr.Add("ImportCoal.Quest.Title", "Importer du charbon");
        _fr.Add("ProduceGunPowder.Quest.Title", "Fabriquer de la poudre à canon");
        _fr.Add("ExportGunPowder.Quest.Title", "Exporter de la poudre à canon");
        _fr.Add("BuildLargeShack.Quest.Title", "Construrie une Grande cabane");
        _fr.Add("BuildA2ndDock.Quest.Title", "Construire un deuxième port");
        _fr.Add("Rename2ndDock.Quest.Title", "Renommer le deuxième port");
        _fr.Add("Import2000Wood.Quest.Title", "Importer du bois");
        _fr.Add("Import2000Coal.Quest.Title", "Importer du charbon");




        _fr.Add("Tutorial.Arrow", "Voici le didacticiel. Une fois terminée, vous gagnerez $10,000");
        _fr.Add("Quest.Arrow", "Voici le bouton de Quête. Vous pouvez accéder à la fenêtre Quête en cliquant dessus");
        _fr.Add("New.Quest.Avail", "Au moins une quête est disponible");
        _fr.Add("Quest_Button.HoverSmall", "Quête");



            //Products
            //Notification.Init()
        _fr.Add("RandomFoundryOutput", "Minerai fondu");

            //OrderShow.ShowToSetCurrentProduct()
        _fr.Add("RandomFoundryOutput (Ore, Wood)", "Minerai fondu (Minerai, Bois)");



            //Bulleting helps
        _fr.Add("Help.Bulletin/Prod/Produce", "Ici est montré ce qui est produit au village.");
        _fr.Add("Help.Bulletin/Prod/Expire", "Ici est montré ce qui a expiré au village.");
        _fr.Add("Help.Bulletin/Prod/Consume", "Ici est montré ce qui est consommé par votre peuple.");

        _fr.Add("Help.Bulletin/Prod/Spec", "Dans cette fenêtre, vous pouvez voir les entrées nécessaires pour chaque produit, où cela se construit et le prix. "
            + "Faites défiler jusqu'au haut pour voir les en-têtes. Notez qu'un simple produit peut avoir plus d'une formule pour le fabriquer.");

        _fr.Add("Help.Bulletin/General/Buildings", "Il s'agit d'un résumé du nombre de bâtiments de chaque type.");

        _fr.Add("Help.Bulletin/General/Workers", "Dans cette fenêtre, vous pouvez affecter des travailleurs à des travaux dans différents bâtiments. "
            + "Pour un bâtiment permettre à plus de personnes dans le travail, doit être inférieure à la capacité et doit trouver au moins une personne sans-emploi.");

        _fr.Add("Help.Bulletin/Finance/Ledger", "Voici votre comptabilité. Le salaire est le montant d'argent versé à un travailleur. Plus les gens travaillent, plus le salaire sera payé.");
        _fr.Add("Help.Bulletin/Finance/Exports", "Une représentation des exportations");
        _fr.Add("Help.Bulletin/Finance/Imports", "Une représentation des importations");


        _fr.Add("Help.Bulletin/Finance/Prices", "....");


        _fr.Add("LoadWontFit", "Ce chargement ne rentre pas dans la zone de stockage");

        _fr.Add("Missing.Input", "Le bâtiment ne peut pas produire (les intrants doivent figurer dans le stockage du bâtiment). Entrées manquantes: \n" );





            //in game
            
        _fr.Add("Buildings.Ready", "\n Bâtiments prêts à être construit:");
        _fr.Add("People.Living", "Personnes vivant dans cette maison:");
        _fr.Add("Occupied:", "Remplis:");
        _fr.Add("|| Capacity:", "|| Capacité:");
        _fr.Add("Users:", "\nUtilisateurs:");
        _fr.Add("Amt.Cant.Be.0", "Le montant ne peut pas être 0");
        _fr.Add("Prod.Not.Select", "Sélectionner un produit");


            //articles
        _fr.Add("The.Male", "Le");
        _fr.Add("The.Female", "La");

            //
        _fr.Add("Build.Destroy.Soon", "Ce bâtiment sera bientôt détruit. Si le stockage n'est pas vide, il doit être vidé par des brouettes");




            //words
            //Field Farms
        _fr.Add("Bean", "Haricot");
        _fr.Add("Potato", "Patate");
        _fr.Add("SugarCane", "Canne à sucre");
        _fr.Add("Corn", "Blé");
        _fr.Add("Cotton", "Coton");
        _fr.Add("Banana", "Banane");
        _fr.Add("Coconut", "Noix de coco");
            //Animal Farm
        _fr.Add("Chicken", "Poulet");
        _fr.Add("Egg", "Oeuf");
        _fr.Add("Pork", "Porc");
        _fr.Add("Beef", "Boeuf");
        _fr.Add("Leather", "Cuir");
        _fr.Add("Fish", "Poisson");
            //mines
        _fr.Add("Gold", "Or");
        _fr.Add("Stone", "Pierre");
        _fr.Add("Iron", "Fer");

            // { "Clay", "Clay");
        _fr.Add("Ceramic", "Céramique");
        _fr.Add("Wood", "Bois");

            //Prod
        _fr.Add("Tool", "Outil");
        _fr.Add("Tonel", "Tonel");
        _fr.Add("Cigar", "Cigare");
        _fr.Add("Tile", "Tuile");
        _fr.Add("Fabric", "Tissu");
        _fr.Add("Paper", "Papier");
        _fr.Add("Map", "Carte");
        _fr.Add("Book", "Livre");
        _fr.Add("Sugar", "Sucre");
        _fr.Add("None", "Aucun");
            //
        _fr.Add("Person", "Personne");
        _fr.Add("Food", "Nourriture");
        _fr.Add("Dollar", "Dollar");
        _fr.Add("Salt", "Sel");
        _fr.Add("Coal", "Charbon");
        _fr.Add("Sulfur", "Souffre");
        _fr.Add("Potassium", "Potassium");
        _fr.Add("Silver", "Argent");
        _fr.Add("Henequen", "Henequen");
            //
        _fr.Add("Sail", "Voile");
        _fr.Add("String", "Corde");
        _fr.Add("Nail", "Clou");
        _fr.Add("CannonBall", "Boulet de canon");
        _fr.Add("TobaccoLeaf", "Sans tabac");
        _fr.Add("CoffeeBean", "Grain de café");
        _fr.Add("Cacao", "Cacao");
        _fr.Add("Weapon", "Arme");
        _fr.Add("WheelBarrow", "Brouette");
        _fr.Add("WhaleOil", "Huile de baleine");
            //
        _fr.Add("Diamond", "Diamand");
        _fr.Add("Jewel", "Bijou");
        _fr.Add("Rum", "Rhum");
        _fr.Add("Wine", "Vin");
        _fr.Add("Ore", "Minerai");
        _fr.Add("Crate", "Caisse");
        _fr.Add("Coin", "Pièce");
        _fr.Add("CannonPart", "Pièce de canon");
        _fr.Add("Steel", "Acier");
            //
        _fr.Add("CornFlower", "Fleur de lys");
        _fr.Add("Bread", "Pain");
        _fr.Add("Carrot", "carotte");
        _fr.Add("Tomato", "Tomate");
        _fr.Add("Cucumber", "Concombre");
        _fr.Add("Cabbage", "Choux-fleur");
        _fr.Add("Lettuce", "Salade");
        _fr.Add("SweetPotato", "patate douce");
        _fr.Add("Yucca", "Yucca");
        _fr.Add("Pineapple", "Ananas");
            //
        _fr.Add("Papaya", "Papaye");
        _fr.Add("Wool", "Laine");
        _fr.Add("Shoe", "Chaussure");
        _fr.Add("CigarBox", "Boîte de cigarre");
        _fr.Add("Water", "Eau");
        _fr.Add("Beer", "Bière");
        _fr.Add("Honey", "Miel");
        _fr.Add("Bucket", "Seau");
        _fr.Add("Cart", "Chariot");
        _fr.Add("RoofTile", "Rooftile");
        _fr.Add("FloorTile", "Dalle");
        _fr.Add("Furniture", "Mobilier");
        _fr.Add("Crockery", "Vaisselle");

        _fr.Add("Utensil", "Utensile");
        _fr.Add("Stop", "Arrêter");


            //more Main GUI
        _fr.Add("Workers distribution", "Répartition des travailleurs");
        _fr.Add("Buildings", "Bâtiments");

        _fr.Add("Age", "Age");
        _fr.Add("Gender", "Genre");
        _fr.Add("Height", "Taille");
        _fr.Add("Weight", "Poids");
        _fr.Add("Calories", "Calories");
        _fr.Add("Nutrition", "Alimentation");
        _fr.Add("Profession", "Profession");
        _fr.Add("Spouse", "Conjoint");
        _fr.Add("Happinness", "Bonheur");
        _fr.Add("Years Of School", "Années d'école");
        _fr.Add("Age majority reach", "Age de la majorité atteint");
        _fr.Add("Home", "Maison");
        _fr.Add("Work", "Travail");
        _fr.Add("Food Source", "Source alimentaire");
        _fr.Add("Religion", "Religion");
        _fr.Add("Chill", "Chilll");
        _fr.Add("Thirst", "Soif");
        _fr.Add("Account", "Compte");

        _fr.Add("Early Access Build", "Développement en accès anticipé");

            //Main Menu
        _fr.Add("Resume Game", "Reprendre la partie");
        _fr.Add("Continue Game", "Continuer la partie");
        _fr.Add("Tutorial", "Didacticiel");
        _fr.Add("New Game", "Nouvelle partie");
        _fr.Add("Load Game", "Charger une partie");
        _fr.Add("Save Game", "Sauvegarder la partie");
        _fr.Add("Achievements", "Réussites");
        _fr.Add("Options", "Options");
        _fr.Add("Exit", "Quitter");
            //Screens
            //New Game
        _fr.Add("Town Name:", "Nom de la ville:");
        _fr.Add("Difficulty:", "Difficulté:");
        _fr.Add("Easy", "Facile");
        _fr.Add("Moderate", "Moyen");
        _fr.Add("Hard", "Difficile");
        _fr.Add("Type of game:", "Type de jeu:");
        _fr.Add("Freewill", "Libre arbitre");
        _fr.Add("Traditional", "Traditionnel");
        _fr.Add("New.Game.Pirates", "Pirates (Si coché, la ville pourrait subir des attaques de Pirates");
        _fr.Add("New.Game.Expires", "Péremption des aliments (Si coché, les aliments se périment)");
        _fr.Add("OK", "OK");
        _fr.Add("Cancel", "Annuler");
        _fr.Add("Delete", "Supprimer");
        _fr.Add("Enter name...", "Saisir un nom...");
            //Options
        _fr.Add("General", "Général");
        _fr.Add("Unit System:", "Unité de mesure:");
        _fr.Add("Metric", "Métrique");
        _fr.Add("Imperial", "Impérial");
        _fr.Add("AutoSave Frec:", "Sauvegarde Auto:");
        _fr.Add("20 min", "20 min");
        _fr.Add("15 min", "15 min");
        _fr.Add("10 min", "10 min");
        _fr.Add("5 min", "5 min");
        _fr.Add("Language:", "Langage:");
        _fr.Add("English", "Anglais");
        _fr.Add("Camera Sensitivity:", "Sensibilité de la Caméra:");
        _fr.Add("Themes", "Thèmes");
        _fr.Add("Halloween:", "Halloween:");
        _fr.Add("Christmas:", "Noël:");
        _fr.Add("Options.Change.Theme", "Relancer le jeu pour appliquer les changements");

        _fr.Add("Screen", "Ecran");
        _fr.Add("Quality:", "Qualité:");
        _fr.Add("Beautiful", "Magnifique");
        _fr.Add("Fantastic", "Fantastique");
        _fr.Add("Simple", "Simple");
        _fr.Add("Good", "Beau");
        _fr.Add("Resolution:", "Résolution:");
        _fr.Add("FullScreen:", "Plein écran:");

        _fr.Add("Audio", "Audio");
        _fr.Add("Music:", "Musique:");
        _fr.Add("Sound:", "Son:");
        _fr.Add("Newborn", "Nouveau-né");
        _fr.Add("Build Completed", "Construction terminée");
        _fr.Add("People's Voice", "Voix Peuple");
            
            //in game gui
        _fr.Add("Prod", "Prod");
        _fr.Add("Finance", "Finance");



            //After Oct 20th 2019
        _fr.Add("Resources", "Ressources");
        _fr.Add("Dollars", "Dollars");
        _fr.Add("Coming.Soon", "This building is coming soon to the game");
        _fr.Add("Max.Population", "Can't build more houses. Max population reached");

        _fr.Add("To.Unlock", "Déverouiller: ");
        _fr.Add("People", "Peuple");
        _fr.Add("Of.Food", " de nourriture. ");//"of food. "
        _fr.Add("Port.Reputation.Least", "Port réputation au moins à ");//"Port reputation at least at "
        _fr.Add("Pirate.Threat.Less", "La menace pirate moins que ");//"Pirate threat less than "
        _fr.Add("Skip", "Sauter");

            //After Dec 8, 2018
        _fr.Add("ReloadMod.HoverSmall", "Recharger des fichiers mod" );



        //
        // 
        // Below It needs to be double checked by Cedric. Dec 17, 2019

        //Dec 14

        //in game gui

        _fr.Add("Help", "Aide" );
        _fr.Add("Quest", "Contesté" );
        _fr.Add("Add Order", "Ordre" );
        _fr.Add("Suggest Change", "Suggestions" );

        _fr.Add("Panel Control / Bulletin", "Panneau de commande" );
        _fr.Add("Exports", "Exportations" );
        _fr.Add("Ledger", "Comptes" );

        _fr.Add("Consume", "Consommée" );
        _fr.Add("Produce", "Produit" );
        _fr.Add("Expire", "Devenue caduque" );

        _fr.Add("Spec", "Spécifique" );
        _fr.Add("Input1", "Apport1" );
        _fr.Add("Input2", "Apport2" );
        _fr.Add("Input3", "Apport3" );
        _fr.Add("Building", "Bâtiment" );
        _fr.Add("Price", "Prix" );

        _fr.Add("Date", "Date" );
        _fr.Add("Product", "Produit" );
        _fr.Add("Amount", "Quantité" );
        _fr.Add("Transaction", "Transaction" );

        _fr.Add("Workers", "Emplois" );

        //Help
        _fr.Add("Bulletin", "Bulletin" );
        _fr.Add("Construction", "Construction" );
        _fr.Add("Happiness", "Félicité" );
        _fr.Add("Horse Carriages", "Carriages" );
        _fr.Add("Inputs", "Billets" );
        _fr.Add("Line production", "La ligne de production" );
        _fr.Add("Our Inventories", "Inventorios" );
        _fr.Add("Inventories Explanation", "Inventorios information" );
        _fr.Add("People Range", "Gamme personnelle" );
        _fr.Add("Pirate Threat", "Menace Pirate" );
        _fr.Add("Population", "Ville" );
        _fr.Add("Port Reputation", "Réputation Port" );
        _fr.Add("Production Tab", "Production Tab" );
        _fr.Add("Products Expiration", "Expiration du produit" );
        _fr.Add("Sea Path", "Route de la mer" );
        _fr.Add("Trading", "Commerce" );
        _fr.Add("Usage of goods", "L'utilisation de produits" );
        _fr.Add("What is Ft3 and M3?", "M3 est ft3?" );
        _fr.Add("WheelBarrows", "Carretilleros" );

          //All Lang Needed for sure
        
        _fr.Add("Unemployed", "Sans emploi" );

        //Budget
        _fr.Add("Budget Resumen", "Comptes" );
        _fr.Add("Initial Balance", "Solde initial" );
        _fr.Add("Income", "Revenu" );
        _fr.Add("Quests Completion", "Résiliés défis" );
        _fr.Add("Income Subtotal", "Sous-total du revenu" );

        _fr.Add("Expenses", "Frais" );
        _fr.Add("New bought lands", "De nouvelles terres achetées" );
        _fr.Add("Salary", "Salaires" );
        _fr.Add("Expenses Subtotal", "Frais Sous-total" );

        _fr.Add("Year", "Année" );
        _fr.Add("Imports", "Année" );
        _fr.Add("Balance", "Équilibre" );
        _fr.Add("Year Balance", "Bilan annuel" );
        _fr.Add("Ending Balance", "Solde final" );

        _fr.Add("Command Keys", "Teclas");
        _fr.Add("Command Keys.Text", "[F1] Ayuda\n[F9] Esconder/Mostrar GUI\n[P] Centra la cámara al Pueblo");
        _fr.Add("Credits", "Créditos");
        _fr.Add("Credits.Text", "Translation:\nCédric Gauché (fr)\nKarsten Eidner (de)");
        _fr.Add("Loading...", "Loading...");

        //Quest window

        _fr.Add("ShowQuest.HoverSmall", "Tarea actual");
        _fr.Add("Have Fun", "Diviértete");
        _fr.Add("Current Quest:", "Tarea actual:");
        _fr.Add("Reward: ", "Premio: ");
        _fr.Add("Reward:", "Premio:");

        _fr.Add("Status: ", "Estado: ");
        _fr.Add("Status", "Estado");
        _fr.Add("Active", "Activa");
        _fr.Add("Done", "Terminada");

        _fr.Add(" of ", " de ");

        //After May 1, 2019
        _fr.Add("Our inventories:", "Nuestros inventarios:");
        _fr.Add("Select Product:", "Selecciona un producto:");
        _fr.Add("Current_Rank.HoverSmall", "Numero en la cola");

        _fr.Add("Construction.Progress", "Progreso de la construcción en: ");
        _fr.Add("Warning.This.Building", "Atención: Esta construcción no puede ser construida ahora. Falta material(es):\n");
        _fr.Add("Product.Selected", "Producto seleccionado: ");
        _fr.Add("Harvest.Date", "\nDia de la recogida: ");
        _fr.Add("Progress", "\nProgreso: ");

        //AddOrderWindow.cs
        _fr.Add("Add.New", "Añade Nueva ");
        _fr.Add("Order", " Orden");
        _fr.Add("Import", "Importa");
        _fr.Add("Export", "Exporta");
        //AddOrderWindow GUI
        _fr.Add("Enter Amount:", "Escribe cantidad:");
        _fr.Add("Enter amount...", "Escribe cantidad...");
        _fr.Add("New Order:", "Orden Nueva:");
        _fr.Add("Product:", "Producto:");
        _fr.Add("Amount:", "Cantidad:");
        _fr.Add("Order total price:", "Precio total de la orden:");
        _fr.Add("Add", "Add");

        //BuildingWindow GUI
        _fr.Add("Product Description:", "Descripción del producto:");
        _fr.Add("Production report by years:", "Reporte productivo:");
        _fr.Add("Import Orders", "Importación");
        _fr.Add("Export Orders", "Exportación");
        _fr.Add("Orders in progress:", "Ordenes actuales:");

        _fr.Add("Notifications", "Notificaciones");

		_fr.Add("Attention.Production", "Atención: la producción se detuvo. Para reanudar la producción en este edificio, seleccione un producto");
		_fr.Add("Selected product: ", "Producto seleccionado: ");
		_fr.Add("Price: ", "Precio: ");
		_fr.Add(" per ", " por ");
		_fr.Add("Inputs needed per ", "Insumos necesarios por ");
		_fr.Add("Inventory:", "Inventario:");

		_fr.Add("CornMeal", "Harina de maíz");
		_fr.Add("PalmLeaf", "Hojas de Palma");
		_fr.Add("Rubber", "Caucho");

        //Dec 9, 2019
        _fr.Add("Barrel", "Barril");
        _fr.Add("Years of school", "Escolaridad");
        _fr.Add("House comfort", "Calidad de Vivienda");
        _fr.Add("Food source", "Bodega");
        _fr.Add("Relax", "Relajamiento");
        _fr.Add("Male", "Masculino");
        _fr.Add("Female", "Femenino");
        _fr.Add("Quenched", "Saciada");


        _fr.Add("Sand", "Arena");
        _fr.Add("Machinery", "Maquinaria");
        _fr.Add("Cassava", "Yuca");
        _fr.Add("Candy", "Caramelo");

        //After Dec 28, 2018
        _fr.Add("Down.HoverSmall", "Prioridad Disminuida");
        _fr.Add("Up.HoverSmall", "Prioridad Aumentada");
        _fr.Add("Trash.HoverSmall", "Borrar Orden");
        _fr.Add("Counting...", "Contando...");
        _fr.Add("Ten Orders Limit", "Diez ordenes es el límite");

    }

    internal static void Clear()
    {
        _fr.Clear();
    }

    public static string ReturnValueWithKey(string key)
    {
        return _fr.ReturnValueWithKey(key);
    }

    public static bool ContainsKey(string key)
    {
        return _fr.ContainsKey(key);
    }

}