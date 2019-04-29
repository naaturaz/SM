using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class French
{
    private static Dictionary<string, string> _french = new Dictionary<string, string>();

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

        _french = new Dictionary<string, string>()
        {
            //Descriptions
            //Infr
            { "Road.Desc","Utilisé à des fins de décoration. Les gens sont plus heureux s'il y a des routes autour d'eux"},
            { "BridgeTrail.Desc","Permet aux gens de passer d'un côté du terrain à l'autre"},
            { "BridgeRoad.Desc","Permet aux gens de passer d'un côté du terrain à l'autre. Les gens adorent ces ponts. Il donne un sentiment de prospérité et de bonheur" +_houseTail},
            { "LightHouse.Desc","Permet d'augmenter la visibilité du port. Ajoute de la notoriété au port tant qu'il y a des travailleurs"},
            { H.Masonry + ".Desc","Bâtiment important, les ouvriers construisent de nouveaux bâtiments et travaillent comme des brouettes une fois qu'ils n'ont rien à faire"},
            { H.StandLamp + ".Desc","Illumine la nuit si l'huile de baleine est disponible dans l'entrepôt de la ville"},

            { H.HeavyLoad + ".Desc","Ces ouvriers utilisent des chariots attelés pour déplacer des marchandises"},


            //House
            { "Bohio.Desc", "Maison Bohio, conditions de vie rudimentaires avec des gens malheureux qui peuvent avoir seulement un maximum de 2 à 3 enfants"},

            { "Shack.Desc", "Cabane, conditions de vie rudimentaires avec des gens malheureux qui peuvent avoir seulement un maximum de 2 enfants"},
            { "MediumShack.Desc", "Moyenne Cabane, est au-dessus des conditions de vie rudimentaires avec un petit bonheur, peut avoir un maximum de 2 à 3 enfants"},
            { "LargeShack.Desc", "Grande Cabane, bonnes conditions de vie avec bonheur, peut avoir un maximum de 2 à 4 enfants"},


            { "WoodHouseA.Desc", "Maison moyenne en bois, une famille peut avoir 2-3 enfants Max" },
            { "WoodHouseB.Desc", "Maison moyenne en bois, une famille peut avoir 3-4 enfants Max"  },
            { "WoodHouseC.Desc", "Maison moyenne en bois, une famille peut avoir 2-3 enfants Max"},
            { "BrickHouseA.Desc", "Maison moyenne, une famille peut avoir 3 enfants Max"},
            { "BrickHouseB.Desc","Grande maison, une famille peut avoir 3-4 enfants Max"},
            { "BrickHouseC.Desc","Grande maison, une famille peut avoir 4 enfants Max"},

            
            //Farms
            //Animal
            { "AnimalFarmSmall.Desc","Petite ferme animale"+_animalFarmTail},
            { "AnimalFarmMed.Desc","Moyenne ferme animale"+_animalFarmTail},
            { "AnimalFarmLarge.Desc","Grande ferme animale"+_animalFarmTail},
            { "AnimalFarmXLarge.Desc","Ferme animale extra-large"+_animalFarmTail},
            //Fields
            { "FieldFarmSmall.Desc","Petit champ agricole"+_fieldFarmTail},
            { "FieldFarmMed.Desc","Moyen champ agricole"+_fieldFarmTail},
            { "FieldFarmLarge.Desc","Grand champ agricole"+_fieldFarmTail},
            { "FieldFarmXLarge.Desc","Champ agricole extra-large"+_fieldFarmTail},
            { H.FishingHut + ".Desc","Un ouvrier peut pêcher ici des poissons dans la rivière (doit être placée près de la rivière)." + _notRegionNeeded},

            //Raw
            { "Mortar.Desc","Un ouvrier produira ici le mortier"},
            { "Clay.Desc","Un ouvrier produira ici l'argile, la matière première est nécessaire pour des briques et plus"},
            { "Pottery.Desc","Un ouvrier produira ici des articles de vaisselles, comme les assiettes, les bocaux, etc"},
            { "Mine.Desc","Un ouvrier peut pêcher ici dans une rivière"},
            { "MountainMine.Desc","Un ouvrier travaillera ici à la mine en prélevant du minerai"},
            { "Resin.Desc","Un ouvrier travaillera ici à la mine en prélevant des minerais et des métaux au hasard"},
            {  H.LumberMill +".Desc","Les ouvriers trouveront ici des ressources telles que le bois, la pierre, et le minerai"},
            { "BlackSmith.Desc","Les travailleurs produiront ici le produit sélectionné"+_asLongHasInput},
            { "ShoreMine.Desc","Les ouvriers produiront ici le sel et le sable"},
            { "QuickLime.Desc","Les ouvriers produiront ici la chaux vive"},

            //Prod
            { "Brick.Desc","Un ouvrier produira ici des produits faits d'argile, tels que des briques, etc."},
            { "Carpentry.Desc","Un ouvrier produira ici des produits faits de bois, tels que des caisses, des tonneaux, etc."},
            { "Cigars.Desc","Les ouvriers produiront ici des cigares"+_asLongHasInput},
            { "Mill.Desc","Les ouvriers vont moudre ici des céréales"+_asLongHasInput},
            { H.Tailor+".Desc","Les ouvriers produits ici des vêtements"+_asLongHasInput},
            { "Tilery.Desc","Les ouvriers produiront ici des tuiles"+_asLongHasInput},
            { "Armory.Desc","Les ouvriers produiront ici des armes"+_asLongHasInput},
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
            { "SugarShop.Desc", "Produit des sous-produits du sucre !!!. " + _produce},


                { "SteelFoundry.Desc",_produce},

            //trade
            { "Dock.Desc","Vous pouvez ajouter ici des commandes d'importation ou d'exportation (doit être placé près de l'océan)." + _notRegionNeeded},
            { H.Shipyard + ".Desc","Vous pouvez réparer vos bâteaux ici, mais vous devez avoir des matériaux de réparation pour bâteaux dans l'inventaire"},
            { "Supplier.Desc","You can supply ships with goods here, but you must have items in inventory that a ship can use for their long trip"},
            { "StorageSmall.Desc",_storage},
            { "StorageMed.Desc",_storage},
            { "StorageBig.Desc",_storage},
            { "StorageBigTwoDoors.Desc",_storage},
            { "StorageExtraBig.Desc",_storage},

            //gov
            { "Library.Desc","Les gens viennent dans ce bâtiment pour lire et emprunter des livres pour leur savoir. Plus les bibliothèques ont de stock, mieux c'est"},
            { "School.Desc","Les gens vont recevoir ici un enseignement"},
            { "TradesSchool.Desc","Les gens vont obtenir ici un enseignement spécialisé pour les métiers"},
            { "TownHouse.Desc","La maison urbaine accroît le bonheur et la prospérité de votre peuple"},

            //other
            { "Church.Desc","L'église donne du bonheur et de l'espoir à votre peuple"},
            { "Tavern.Desc","La taverne donne un peu de détente et de divertissement à votre peuple"},

            //Militar
            { "WoodPost.Desc", "Ils repèrent les bandits et les pirates plus rapidement afin que vous puissiez vous préparer à l'avance"},
            { "PostGuard.Desc",_militar},
            { "Fort.Desc",_militar},
            { "Morro.Desc",_militar+". Une fois que vous construisez ceci, les pirates devraient le savoir"},

            //Decoration
            { "Fountain.Desc", "Sublime votre ville et ajoute du bonheur global à vos citoyens"},
            { "WideFountain.Desc", "Sublime votre ville et ajoute du bonheur global à vos citoyens"},
            { "PalmTree.Desc", "Sublime votre ville et ajoute du bonheur global à vos citoyens"},
            { "FloorFountain.Desc", "Sublime votre ville et ajoute du bonheur global à vos citoyens"},
            { "FlowerPot.Desc", "Sublime votre ville et ajoute du bonheur global à vos citoyens"},
            { "PradoLion.Desc", "Sublime votre ville et ajoute du bonheur global à vos citoyens"},



            //Buildings name
            //Infr
            { "Road","Route"},
            { "BridgeTrail","Pont de sentier"},
            { "BridgeRoad","Pont routier"},
            { "LightHouse","Phare"},
            { "Masonry","Maçonnerie"},
            {   "StandLamp","Lampadaire"},
            { "HeavyLoad","Charge lourde"},


            //House
            { "Shack", "Cabane"},
            { "MediumShack", "Moyenne cabane"},
            { "LargeShack", "Grande cabane"},

            { "WoodHouseA", "Moyenne maison en bois" },
            { "WoodHouseB", "Grande maison en bois"  },
            { "WoodHouseC", "Maison en bois de luxe"},
            { "BrickHouseA", "Moyenne maison de brique"},
            { "BrickHouseB","Maison de brique de luxe"},
            { "BrickHouseC","Grande maison de brique"},

            
            //Farms
            //Animal
            { "AnimalFarmSmall","Petite ferme animale"},
            { "AnimalFarmMed","Moyenne ferme animale"},
            { "AnimalFarmLarge","Grande ferme animale"},
            { "AnimalFarmXLarge","Ferme animale extra-large"},
            //Fields
            { "FieldFarmSmall","Petit champ agricole"},
            { "FieldFarmMed","Moyen champ agricole"},
            { "FieldFarmLarge","Grand champ agricole"},
            { "FieldFarmXLarge","Champ agricole extra-large"},
            { "FishingHut","Hutte de pêcheur"},

            //Raw
            { "Mortar","Mortier"},
            { "Clay","Argile"},
            { "Pottery","Poterie"},
            { "MountainMine","Mine de montagne"},
            {  "LumberMill" ,"Scierie"},
            { "BlackSmith","Forge"},
            { "ShoreMine","Mine de littoral"},
            { "QuickLime","Chaux vive"},

            //Prod
            { "Brick","Brique"},
            { "Carpentry","Menuiserie"},
            { "Cigars","Cigares"},
            { "Mill","Moulin"},
            { "Tailor","Tailleur"},
            { "Tilery","Tuilerie"},
            { "Armory","Armurerie"},
            { "Distillery","Distillerie"},
            { "Chocolate","Chocolaterie"},
            { "Ink","Encre"},

            //Ind
            { "Cloth","Vêtement"},
            { "GunPowder","Poudre à canon"},
            { "PaperMill","Papeterie"},
            { "Printer","Printer"},
            { "CoinStamp","Pièce de monnaie"},
            { "SugarMill","Usine sucrière"},
            { "Foundry","Fonderie"},
            { "SteelFoundry","Fonderie d'acier"},
            { "SugarShop","SugarShop"},


            //trade
            { "Dock","Port"},
            { "Shipyard","Chantier naval"},
            { "Supplier","Fournisseur"},
            { "StorageSmall","Petit entrepôt"},
            { "StorageMed","Moyen entrepôt"},
            { "StorageBig","Grand entrepôt"},

            //gov
            { "Library","Bibliothèque"},
            { "School","Ecole"},
            { "TradesSchool","Ecole de métiers"},
            { "TownHouse","Maison urbaine"},

            //other
            { "Church","Eglise"},
            { "Tavern","Taverne"},

            //Militar
            { "WoodPost", "Wood Guard Duty"},
            { "PostGuard","Stone Guard Duty"},
            { "Fort","Fort"},
            { "Morro","Morro"},

            //Decorations
            { "Fountain", "Fontaine"},
            { "WideFountain", "Grande fontaine"},
            { "PalmTree", "Palmier"},
            { "FloorFountain", "Floor Fountain"},
            { "FlowerPot", "Pot de fleurs"},
            { "PradoLion", "Lion du Prado"},

            //Main GUI
            { "SaveGame.Dialog", "Enregistrer votre progression de jeu"},
            { "LoadGame.Dialog", "Charger une partie"},
            { "NameToSave", "Enregistrer votre partie en:"},
            { "NameToLoad", "Partie à charger sélectionnée:"},
            { "OverWrite", "Il y a déjà une partie enregistrée avec le même nom. Voulez-vous remplacer le fichier?"},
            { "DeleteDialog", "Êtes-vous sûr de vouloir supprimer la partie enregistrée?"},
            { "NotHDDSpace", "Pas assez d'espace sur le lecteur {0} pour enregistrer la partie"},
            { "GameOverPirate", "Désolé, vous avez perdu la partie! Les pirates ont attaqué votre ville et tué tout le monde."},
            { "GameOverMoney", "Désolé, vous avez perdu la partie! La Couronne ne soutiendra plus votre île des Caraïbes."},
            { "BuyRegion.WithMoney", "Êtes-vous sûr de vouloir acheter cet emplacement?."},
            { "BuyRegion.WithOutMoney", "Désolé, vous ne pouvez pas vous le permettre maintenant."},
            { "Feedback", "Un avis!? Super...:) Merci. 8) "},
            { "OptionalFeedback", "Les avis sont importants. SVP laissez un commentaire "},
            { "MandatoryFeedback", "Seulement l'équipe des développeurs verront ceci. Votre note est?"},
            { "PathToSeaExplain", "Affiche le chemin le plus court vers la mer."},


            { "BugReport", "Vous avez trouvé un bug? uhmm miam-miam.... Envoyez-le nous!! Merci"},
            { "Invitation", "L'email de votre ami pour avoir une chance de rejoindre la Beta privée"},
            { "Info", ""},//use for informational Dialogs
            { "Negative", "La Couronne vous a accordé une marge de crédit. Si vous possédez plus de $100 000,00 c'est Game Over"},  


            //MainMenu
                { "Types_Explain", "Traditional: \nC'est un jeu où, au début, certains bâtiments sont verrouillés et vous devez les déverrouiller. " +
                    "La bonne nouvelle c'est que vous aurez des conseils." +
                    "\n\nFreewill: \nTous les bâtiments disponibles sont déverouillés tout de suite. " +
                    "La mauvaise nouvelle c'est que de cette façon vous pouvez échouer très facilement." +
                    "\n\nLa difficulté 'Difficile' est la plus proche de la réalité"},


            //Tooltips
            //Small Tooltips
            { "Person.HoverSmall", "Total/Adultes/Enfants"},
            { "Emigrate.HoverSmall", "Emigré"},
            { "CurrSpeed.HoverSmall", "Vitesse de jeu"},
            { "Town.HoverSmall", "Nom de la ville"},
            { "Lazy.HoverSmall", "Sans-emploi"},
            { "Food.HoverSmall", "Nourriture"},
            { "Happy.HoverSmall", "Bonheur"},
            { "PortReputation.HoverSmall", "Notoriété du port"},
            { "Dollars.HoverSmall", "Dollars"},
            { "PirateThreat.HoverSmall", "Menace pirate"},
            { "Date.HoverSmall", "Date (m/a)"},
            { "MoreSpeed.HoverSmall", "Augmenter [PgUp]"},
            { "LessSpeed.HoverSmall", "Diminuer [PgDwn]"},
            { "PauseSpeed.HoverSmall", "Mettre en pause"},
            { "CurrSpeedBack.HoverSmall", "Vitesse actuelle"},
            { "ShowNoti.HoverSmall", "Notifications"},
            { "Menu.HoverSmall", "Menu principal"},
            { "QuickSave.HoverSmall", "Sauvegarde rapide[Ctrl+S][F]"},
            { "Bug Report.HoverSmall", "Signaler un bug"},
            { "Feedback.HoverSmall", "Commentaire"},
            { "Hide.HoverSmall", "Cacher"},
            { "CleanAll.HoverSmall", "Nettoyer"},
            { "Bulletin.HoverSmall", "Contrôle/Bulletin"},
            { "ShowAgainTuto.HoverSmall","Didacticiel"},
            { "BuyRegion.HoverSmall", "Acheter des emplacements"},
            { "Help.HoverSmall", "Aider"},

            { "More.HoverSmall", "Plus"},
            { "Less.HoverSmall", "Moins"},
            { "Prev.HoverSmall", "Précédent"},

            { "More Positions.HoverSmall", "Plus"},
            { "Less Positions.HoverSmall", "Moins"},


            //down bar
            { "Infrastructure.HoverSmall", "Infrastructure"},
            { "House.HoverSmall", "Logement"},
            { "Farming.HoverSmall", "Agriculture"},
            { "Raw.HoverSmall", "Matière première"},
            { "Prod.HoverSmall", "Production"},
            { "Ind.HoverSmall", "Industrie"},
            { "Trade.HoverSmall", "Commerce"},
            { "Gov.HoverSmall", "Gouvernement"},
            { "Other.HoverSmall", "Autre"},
            { "Militar.HoverSmall", "Militaire"},
            { "Decoration.HoverSmall", "Décoration"},

            { "WhereIsTown.HoverSmall", "Retourner à la ville [P]"},
            { "WhereIsSea.HoverSmall", "Afficher Ocean Path"},
            { "Helper.HoverSmall", "Aide"},
            { "Tempeture.HoverSmall", "Température"},
            
            //building window
            { "Gen_Btn.HoverSmall", "Onglet Général"},
            { "Inv_Btn.HoverSmall", "Onglet Inventaire"},
            { "Upg_Btn.HoverSmall", "Onglet Améliorations"},
            { "Prd_Btn.HoverSmall", "Onglet Production"},
            { "Sta_Btn.HoverSmall", "Onglet Statistiques"},
            { "Ord_Btn.HoverSmall", "Onglet Commandes"},
            { "Stop_Production.HoverSmall", "Arrêter la production"},
            { "Demolish_Btn.HoverSmall", "Démolir"},
            { "More Salary.HoverSmall", "Payer plus"},
            { "Less Salary.HoverSmall", "Payer moins"},
            { "Next_Stage_Btn.HoverSmall", "Acheter le prochain niveau"},
            { "Current_Salary.HoverSmall", "Salaire actuel"},
            { "Current_Positions.HoverSmall", "Positions actuelles"},
            { "Max_Positions.HoverSmall", "Positions max"},


            { "Add_Import_Btn.HoverSmall", "Ajouter une importation"},
            { "Add_Export_Btn.HoverSmall", "Ajouter une exportation"},
            { "Upg_Cap_Btn.HoverSmall", "Améliorer les capacités"},
            { "Close_Btn.HoverSmall", "Fermer"},
            { "ShowPath.HoverSmall", "Montrer le chemin"},
            { "ShowLocation.HoverSmall", "Afficher l'emplacement"},//TownTitle
            { "TownTitle.HoverSmall", "Ville"},
            {"WarMode.HoverSmall", "Mode Combat"},
            {"BullDozer.HoverSmall", "Bulldozer"},
            {"Rate.HoverSmall", "Rate Me"},

            //addOrder windiw
            { "Amt_Tip.HoverSmall", "Quantité de produit"},

            //Med Tooltips 
            { "Build.HoverMed", "Placer un bâtiment: 'Clic Gauche' \n" +
                                "Pivoter un bâtiment: Touche 'R' \n" +
                                "Annuler: 'Clic Droit'"},
                { "BullDozer.HoverMed", "Nettoyer une zone: 'Clic Gauche' \n" + 
                "Annuler: 'Clic Droit' \nCoût: $10 par utilisation "},

                { "Road.HoverMed", "Commencer: 'Clic Gauche' \n" +
                    "Agrandir: 'Bouger la souris' \n" +
                    "Fixer: 'Clic Gauche encore' \n" +
                "Annuler: 'Clic Droit'"},

            { "Current_Salary.HoverMed", "Les travailleurs iront au travail, où le salaire est le plus élevé." +
                                            " Si 2 places offrent le même salaire, alors l'endroit le plus proche de la maison sera choisi."},



            //Notifications
            { "BabyBorn.Noti.Name", "Nouveau-né"},
            { "BabyBorn.Noti.Desc", "{0} est né"},
            { "PirateUp.Noti.Name", "Les Pirates sont proche"},
            { "PirateUp.Noti.Desc", "Les Pirates sont proche du rivage"},
            { "PirateDown.Noti.Name", "Les Pirates vous respectent"},
            { "PirateDown.Noti.Desc", "Les Pirates vous respectent un peu plus aujourd'hui"},

            { "Emigrate.Noti.Name", "Un citoyen a émigré"},
            { "Emigrate.Noti.Desc", "Les gens émigrent quand ils ne sont pas satisfaits de votre gouvernement"},
            { "PortUp.Noti.Name", "Le port est connu"},
            { "PortUp.Noti.Desc", "Votre notoriété portuaire est en hausse avec les ports voisins et les routes"},
            { "PortDown.Noti.Name", "Le port est moins connu"},
            { "PortDown.Noti.Desc", "Votre notoriété portuaire a baissée"},

            { "BoughtLand.Noti.Name", "Nouveau terrain acheté"},
            { "BoughtLand.Noti.Desc", "Une nouvelle zone foncière a été achetée"},

            { "ShipPayed.Noti.Name", "Bâteau payé"},
            { "ShipPayed.Noti.Desc", "Un bâteau a payé {0} pour des biens ou des services"},
            { "ShipArrived.Noti.Name", "Un bâteau est arrivé"},
            { "ShipArrived.Noti.Desc", "Un nouveau bâteau est arrivé à l'un de nos bâtiments maritimes"},

            { "AgeMajor.Noti.Name", "Nouveau travailleur"},
            { "AgeMajor.Noti.Desc", "{0} est prêt à travailler"},


            { "PersonDie.Noti.Name", "Une personne est décédée"},
            { "PersonDie.Noti.Desc", "{0} est mort"},

            { "DieReplacementFound.Noti.Name", "Une personne est décédée"},
            { "DieReplacementFound.Noti.Desc", "{0} est mort. Un remplaçant pour l'emploi a été trouvé."},

            { "DieReplacementNotFound.Noti.Name", "Une personne est décédée"},
            { "DieReplacementNotFound.Noti.Desc", "{0} est mort. Aucun remplaçant pour le poste a été trouvé"},


            { "FullStore.Noti.Name", "Un entrepôt déborde"},
            { "FullStore.Noti.Desc", "Un entrepôt est à {0}% capacités"},

            { "CantProduceBzFullStore.Noti.Name", "Un ouvrier ne peut pas produire"},
            { "CantProduceBzFullStore.Noti.Desc", "{0} parce que l'entrepôt cible est complet"},

            { "NoInput.Noti.Name", "Au moins une entrée est manquante dans la construction"},
            { "NoInput.Noti.Desc", "Un bâtiment ne peut pas produire {0} car il manque au moins une entrée"},

            { "Built.Noti.Name", "Un bâtiment a été construit"},
            { "Built.Noti.Desc", "{0} a été entièrement construit"},

            { "cannot produce", "ne peut pas produire"},

            



            //Main notificaion
            //Shows on the middle of the screen
            { "NotScaledOnFloor", "Le bâtiment est soit trop près de la rive ou de la montagne"},
            { "NotEven", "Le sol sous le bâtiment n'est même pas"},
            { "Colliding", "Le bâtiment est en collision avec un autre"},
            { "Colliding.BullDozer", "Le bulldozer est en collision avec un bâtiment. Ne peut être utilisé sur le terrain (arbres, roches)"},

            { "BadWaterHeight", "Le bâtiment est trop bas ou haut sur l'eau"},
            { "LockedRegion", "Vous devez posséder cette zone pour construire ici"},
            { "HomeLess", "Les gens de cette maison n'ont nulle part où aller. SVP construiser une nouvelle maison" +
                            " peut contenir cette famille et essayer à nouveau"},
            { "LastFood", "Impossible de détruire, c'est le seul entrepôt dans votre village"},
            { "LastMasonry", "Impossible de détruire, c'est la seule maçonnerie dans votre village"},
            { "OnlyOneDemolish", "Vous démolissez déjà un bâtiment. Essayer à nouveau après que la démolition soit terminée"},


            //help

            { "Construction.HoverMed", "Pour la construction d'un bâtiment, vous devez avoir des ouvriers dans la maçonnerie. "+
                    " Cliquez sur la maçonnerie puis sur le signe '+' dans l'onglet général. Assurez-vous d'avoir suffisamment de ressources"},
            { "Demolition.HoverMed", "Once the inventory is clear will be demolished. Les brouettes déplaceront le stock"},

            { "Construction.Help", "Pour la construction d'un bâtiment, vous devez avoir des ouvriers dans la maçonnerie. "+
                    " Cliquez sur la maçonnerie, puis sur le signe '+' dans l'onglet général. Assurez-vous d'avoir suffisamment de ressources"},
            { "Camera.Help", "Camera: Utiliser [WASD] ou le curseur pour la déplacer. " +
                        "Utilisez la molette de défilement de votre souris, maintenez-la enfoncée pour faire pivoter, ou [Q] et [E]"},
            { "Sea Path.Help", "Cliquez sur le coin inférieur gauche 'Afficher/masquer le chemin de la mer' " +
                            "bouton pour afficher le chemin le plus proche de la mer"},

            { "People Range.Help", "L'énorme cercle bleu autour de chaque bâtiment désigne sa portée"},

            { "Pirate Threat.Help", "Menace Pirate: C'est ainsi que les Pirates de votre port sont informés. Cela augmente comme " +
                                        " Vous avez plus d'argent. Si cela atteint plus de 90, vous perdrez la partie. Vous pouvez contrer la menace en construisant des bâtiments militaires"},

            { "Port Reputation.Help", "Notoriété du port: Plus les gens connaissent votre port, plus ils vont le visiter." +
                                            " Si vous voulez l'augmenter, assurez vous que vous avez toujours des commandes" +
                                            " dans le port"},
            { "Emigrate.Help", "Emigré: Quand les gens sont malheureux pendant quelques années, ils partent. Le pire" +
                                    " c'est qu'ils ne reviendront pas, ils ne produiront rien ou n'auront pas d'enfants." +
                                    " La bonne nouvelle c'est qu'ils augmentent la 'notoriété du port'"},
            { "Food.Help", "Nourriture: Plus la variété d'aliments disponible dans un ménage est grande, plus ils seront" +
                                " heureux."},

            { "Weight.Help", "Poids: Tous les poids dans le jeu sont en kg ou lbs selon le système d'unité qui est sélectionné." +
                                " Vous pouvez le changer dans les options dans le menu principal'"},
            { "What are Ft3 and M3?.Help", "La capacité de stockage est déterminée par le volume du bâtiment. Ft3 c'est un cubic foot. M3 c'est un mètre cube" },//. Keep in mind that less dense products will fill up your storage quickly. To see products density Bulletin/Prod/Spec"},

            { "More.Help", "Si vous avez besoin d'aides supplémentaires, il serait peut être utile de terminer le didacticiel, ou tout simplement poster une question sur les forums de SugarMill's"},

                //more 
            { "Products Expiration.Help", "Péremption des produits: Tout comme dans la vie réelle, dans ce jeu, chaque produit se périme. Certains produits alimentaires se périment plus tôt que d'autres. Vous pouvez voir combien de produits ont périmés dans le Bulletin/Prod/Péremption"},
            { "Horse Carriages.Help", "Comme le jeu a des mesures réelles les gens ne peuvent pas transporter de trop. C'est alors que les chariots tirés par des chevaux entrent en place. Ils transportent beaucoup plus, en conséquence, votre économie est boostée. Une personne dans ses meilleures années pourrait porter environ 15 kg, une brouette proche des 60KG, mais le plus petit chariot peut transporter 240KG. Pour les utiliser, construisez un HeavyLoad "},
            { "Usage of goods.Help", "Utilisation des marchandises: Les caisses, les tonneaux, les brouettes, les chariots, les outils, les vêtements, la vaisselle, les meubles et les ustensiles sont tous nécessaires pour faire les activités traditionnelles d'une ville. Comme ces marchandises s'usent, elles diminuent, par conséquent, une personne ne portera rien s'il n'y a pas de caisses. Gardez y un œil ;)"},
            { "Happiness.Help", "Bonheur: Le bonheur des gens est influencé par différents facteurs. Le nombre d'argent qu'ils possèdent, la variété de nourriture, la satisfaction de la religion, l'accès aux loisirs, le confort de la maison et le niveau d'éducation. Aussi, si une personne a accès à des ustensiles, la vaisselle et les vêtements influenceront leur bonheur."},
            { "Line production.Help", "Production à la chaîne: Pour fabriquer un simple clou, vous avez besoin de minerai de la mine, dans la fonderie fondre du fer, et enfin à la forge fabriquer le clou. Si vous avez assez d'argent, vous pouvez toujours acheter le clou directement sur un bâteau, ou tout autre produit."},
            { "Bulletin.Help", "L'icône de pages de la barre inférieure est la fenêtre Bulletin/contrôle. Prenez une minute pour l'explorer."},
            { "Trading.Help", "Vous devez avoir au moins un port pour faire du commercer. Au port, vous pouvez ajouter des commandes d'importation/exportation et faire de l'argent comptant. Si vous avez besoin d'aide pour ajouter une commande, il serait peut-être utile de compléter le didacticiel"},

            { "Combat Mode.Help", "Il s'active quand un Pirate/Bandit est détecté par l'un de vos citoyens. Une fois que le mode est actif, vous pouvez commander des unités directement à l'attaque. Sélectionnez-les et cliquez avec le bouton droit pour cibler pour attaquer"},

            { "Population.Help", "Une fois qu'ils ont 16 ans, ils se déplaceront dans une maison libre s'ils en trouvent. S'il y a toujours une maison libre pour augmenter la croissance, la population sera garantie. S'ils entrent dans les nouvelles maisons à 16 ans, vous Maximisez la croissance de la population"},


            { "F1.Help", "Appuyez sur [F1] pour l'Aide"},

            { "Inputs.Help", "Si un bâtiment ne peut pas produire parce qu'il manque des entrées. Vérifiez que vous avez l'entrée (s) nécessaire (s) dans l'entrepôt principal et au moins un ouvrier en maçonnerie"},
            { "WheelBarrows.Help", "Les brouettes sont les ouvriers de maçonnerie. S'ils n'ont rien à construire, ils agiront comme des brouettes. Si vous avez besoin d'entrées pour entrer dans un bâtiment spécifique Assurez-vous d'en avoir assez qui travaillent et vérifiez aussi les entrées mentionnées dans le bâtiment de stockage"},

            { "Production Tab.Help", "Si le bâtiment est un champ agricole, assurez-vous que vous avez des travailleurs à la ferme. La récolte sera perdue si elle se tient un mois après le jour de la récolte"},
            { "Our Inventories.Help", "La section 'Nos stocks' dans la fenêtre 'Ajouter une Commande' est un résumé de ce que nous avons obtenu dans nos stocks de bâtiments de stockage"},
            { "Inventories Explanation.Help", "Voici un résumé de ce que nous avons obtenu dans nos stocks. Les articles dans d'autres bâtiments de stockage n'appartiennent pas à la ville"},

            ///word and grammarly below




            //to  add on spanish         //to correct  
            { "TutoOver", "Votre récompense est de $10 000,00 si c'est la première fois que vous le terminez. Le didacticiel est terminé maintenant vous pouvez continuer à jouer à cette partie ou en commencer une nouvelle."},

            //Tuto
            { "CamMov.Tuto", "La récompense de fin du didacticiel est de $10 000 (une seule récompense par partie). Etape 1: utilisez [WASD] ou les touches fléchées pour déplacer la caméra. Faites-le pendant au moins 5 secondes"},
            { "CamMov5x.Tuto", "Utilisez [WASD] ou les touches fléchées et maintenez la touche Shift gauche enfoncée pour déplacer la caméra 5 fois plus vite. Faites-le pendant au moins 5 secondes"},
            { "CamRot.Tuto", "Maintenant, appuyez sur la molette de défilement vers le bas sur votre souris et déplacez votre souris pour faire pivoter la caméra. Faites-le pendant au moins 5 secondes"},


            { "BackToTown.Tuto", "Appuyez sur la touche [P] du clavier pour accéder à la position initiale de la caméra."},

            { "BuyRegion.Tuto", "ZONES: Vous devez posséder une zone pour pouvoir y construire. Cliquez sur le signe '+' sur la barre inférieure, puis sur le signe 'A Vendre' dans le" + 
                    " milieu d'une zone pour l'acheter. Certains bâtiments peuvent être construits sans posséder la zone (Hutte de pêcheur, Port," + 
                    " Mine de montagne, Mine du littoral, Lampadaire, Poste de guarde)"
                    },

            { "Trade.Tuto", "C'était facile, la partie la plus difficile est à venir. Cliquez sur le bouton 'Commerce', situé dans la barre inférieure droite. "+ 
                "Survolez-le, un menu contextuel 'Commerce' apparaîtra"},
            { "CamHeaven.Tuto", "Faites un défilement arrière avec le bouton central de la souris jusqu'à ce que la caméra atteigne"
                    + " le ciel. Cette vue est utile pour placer de plus grands bâtiments tels que le port"},

            { "Dock.Tuto", "Maintenant, cliquez sur le 'Port', c'est le premier bouton. Lorsque vous le survolez, il affichera"+
                " sont coût et sa description"},
            { "Dock.Placed.Tuto", "Maintenant, plus difficile, lisez attentivement. Notez que vous pouvez utiliser la "+
                " touche 'R' pour faire pivoter, et faites un clic droit pour annuler le bâtiment. Ce bâtiment a une partie dans l'océan et une autre sur la terre." +
                " La flèche va en direction de la mer, la zone de stockage va sur terre. Une fois que la flèche est de couleur blanche, clic gauche."},
            { "2XSpeed.Tuto", "Augmentez la vitesse du jeu, accédez au contrôleur de vitesse de simulation d'écran du milieu supérieur, cliquez sur le "
                    +" bouton 'Plus de Vitesse' 1 fois jusqu'à ce que 2x soit affiché"},

            { "ShowWorkersControl.Tuto", "Cliquez sur le bouton 'Contrôle/Bulletin', situé dans la barre inférieure. "+ 
                "Survolez-le, un menu contextuel 'Contrôle/Bulletin' apparaîtra"},
            { "AddWorkers.Tuto", "Cliquez sur le signe' + 'à droite du bâtiment de maçonnerie, c'est ainsi que vous ajoutez plus de travailleurs."},
            { "HideBulletin.Tuto", "Gardez à l'esprit que dans cette fenêtre vous pouvez contrôler et voir différents aspects du jeu. Cliquez à l'extérieur de la fenêtre pour la fermer ou sur le bouton 'OK'."},
            { "FinishDock.Tuto", "Terminez maintenant la construction du port. Plus il y a de travailleurs en maçonnerie, plus cela sera rapide."
            + "Assurez-vous également que vous avez tous les matériaux nécessaires pour le construire"},
            { "ShowHelp.Tuto", "Cliquez sur le bouton 'Aide', situé dans la barre inférieure. "+
                "Survolez-le, un menu contextuel 'Aide' apparaîtra. Là, vous y trouverez quelques conseils."},


            { "SelectDock.Tuto", "Les bâteaux déposent et ramassent des marchandises au hasard via les stocks du port. Les ouvriers sont nécessaires pour déplacer des marchandises d'arrimage dedans et dehors. Ils ont besoin de brouettes et de caisses. Si aucun élément ne se trouve dans le stockage du port, ils ne fonctionneront pas. Maintenant, cliquez sur le port."},


            { "OrderTab.Tuto", "Accédez à l'onglet commandes de la fenêtre du port."},
            { "ImportOrder.Tuto", "Cliquez sur le signe '+' à côté du bouton Ajouter une Commande d'Importation."},

            { "AddOrder.Tuto", "Faites défiler les produits vers le bas, sélectionnez le bois et entrez 100 comme montant. Cliquez ensuite sur le bouton 'ajouter'."},
            { "CloseDockWindow.Tuto", "Maintenant, la commande est ajoutée. Un bâteau aléatoire déposera cet élément dans le stockage du port. Ensuite, vos ouvriers du port l'emmèneront à l'entrepôt le plus proche. Maintenant, cliquez hors de la fenêtre, de sorte qu'elle se ferme."},
            { "Rename.Tuto", "Cliquez sur une personne, puis cliquez sur la barre de titre de la personne. Vous pouvez changer le nom de n'importe quelle personne ou bâtiment dans le jeu. Cliquez à l'extérieur pour que la modification soit sauvegardée "},
            { "RenameBuild.Tuto", "Cliquez maintenant sur un bâtiment et changez son nom de la même manière. N'oubliez pas de cliquer à l'extérieur de sorte que le changement soit enregistré"},

            { "BullDozer.Tuto", "Cliquez maintenant sur l'icône bulldozer dans la barre inférieure. Retirez un arbre ou une roche du relief."},


            { "Budget.Tuto", "Cliquez sur le bouton 'Contrôle/Bulletin', puis sur le menu 'Finance', puis sur 'Registre'. Il s'agit du grand-livre du jeu"},
            { "Prod.Tuto", "Cliquez sur le menu 'Prod' puis sur 'produire'. Cela montrera la production du village pour les 5 dernières années"},
            { "Spec.Tuto", "Cliquez sur le menu 'Prod', puis sur 'Spec'. Ici vous pouvez voir exactement comment fabriquer chaque produit sur le jeu. Les intrants nécessaires et où cela est produit. Aussi, les prix à l'importation et à l'exportation "},
            { "Exports.Tuto", "Cliquez sur le menu 'Finances' puis sur 'Exporter'. Ici vous pouvez voir une répartition des exportations de votre village "},


                //Quest
            { "Tutorial.Quest", "Quête: Terminer le didacticiel. Récompense de $10 000. Il faut environ 3 minutes pour le terminer"},

            { "Lamp.Quest", "Quête: Construire un lampadaire. Trouvez-le dans l'infrastructure, il brille la nuit si il ya de l'huile de baleine dans l'entrepôt"},

            { "Shack.Quest", "Quête: Construire une cabane. Ce sont des maisons bon marché. Quand les gens ont 16a, ils se déplacent vers une maison de libre si ils en trouvent. De cette façon, la croissance de la population sera garantie. [F1] Aide. Si vous voyez de la fumée dans la cheminée d'une maison, cela signifie qu'il ya des gens qui y vivent "},

            { "SmallFarm.Quest", "Quête: Construire une petite ferme agricole. Vous avez besoin de fermes pour nourrir votre peuple"},
            { "FarmHire.Quest", "Quête: Engager deux fermiers dans la petite ferme agricole. Cliquez sur la ferme et dans le signe plus affecter les travailleurs. Vous devez avoir des gens"
                    +" sans emploi pour pouvoir les assigner dans un nouveau bâtiment"},



            { "FarmProduce.Quest", "Quête: Produisons " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " des haricots dans la petite ferme agricole. Cliquez sur l'onglet 'Stat' et cela vous montrera la production des 5 dernières années. Vous pouvez voir le progrès de la quête dans la fenêtre de quête. Si vous construisez plus de petites fermes elles seront comptabilisés pour la quête"},
            { "Transport.Quest", "Quête: Transporter les haricots de la ferme jusqu'à l'entrepôt. Pour ce faire, assurez-vous qu'" +
                " ils y a des ouvriers en maçonnerie. Ils agissent comme des brouettes quand ils ne construisent pas"},


            { "HireDocker.Quest", "Quête: Embaucher un docker. La seule tâche des dockers est de déplacer les marchandises dans le port via l'entrepôt si vous exportez."+
            " Ou vice-versa en cas d'importation. Ils travaillent lorsqu'il y a un ordre en place et que les marchandises sont prêtes à être transportées. Sinon, ils restent à la maison au repos." +
                " Si vous n'avez pas construit de port, construisez-en un."+
            " Rechercher le dans le Commerce." },


            { "Export.Quest", "Quête: Au port, créez une commande et exportez exactement 300 " + Unit.CurrentWeightUnitsString() + " d'haricots."+
                " Au port, cliquez sur l'onglet 'Commandes' et ajoutez une commande d'exportation avec le signe '+'."+
            " Sélectionnez le produit et entrez le montant"},



            { "MakeBucks.Quest", "Quête: Avoir $100 en exporant des marchandises au port. "+
            "Une fois qu'un bâteau arrive, il paiera de façon aléatoire le produit(s) dans le stockage de votre port"},
            { "HeavyLoad.Quest", "Quête: Construire un bâtiment HeavyLoad. Ce sont des transporteurs qui portent plus de poids. Ils seront utiles lors du transport des marchandises." }, //Carts must be available on towns storages for them to work"},
            { "HireHeavy.Quest", "Quête: Dans le bâtiment HeavyLoad, embaucher un transporteur lourd."},


            { "ImportOil.Quest", "Quête: Importer 500 " + Unit.CurrentWeightUnitsString() + " d'huiles de baleine au port. Il est nécessaire de garder vos lumières allumées la nuit. Les bâteaux livreront les importations au hasard dans le stockage du port"},

            { "Population50.Quest", "Atteindre une population totale de 50 citoyens"},

            //added Aug 11 2017, result: sep 9(30% off biggest sale ever)
            { "Production.Quest", "Nous allons produire des armes maintenant et les vendre plus tard. Tout d'abord, construisez une forge. Trouvez-la dans le menu 'Matières Premières' des bâtiments"},
            { "ChangeProductToWeapon.Quest", "Dans l'onglet 'produits' de la forge, changez la production en arme. Les ouvriers apporteront les matières premières nécessaires pour forger des armes s'ils en trouvent"},
            { "BlackSmithHire.Quest", "Embaucher 2 forgerons"},
            { "WeaponsProduce.Quest", "Produisons " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " d'armes à la forge. Cliquez sur l'onglet 'Stat' et cela vous montrera la production des 5 dernières années. Vous pouvez voir la progression de la quête dans la fenêtre Quête."},
            { "ExportWeapons.Quest", "Exportons 100 " + Unit.CurrentWeightUnitsString() + " d'armes. Au port, ajoutez une commande d'exportation. Notez que la vente d'arme est une entreprise rentable."},


            {"CompleteQuest", "Votre récompense est {0}"},


            //added Sep 14 2017
            { "BuildFishingHut.Quest", "Construisez une butte de pêcheur. De cette façon, les citoyens ont des aliments différents à manger, ce qui se traduit par du bonheur"},
            { "HireFisher.Quest", "Embaucher un pêcheur"},

            { "BuildLumber.Quest", "Construisez une scierie. Trouvez-la dans le menu 'Matières Premières' des bâtiments"},
            { "HireLumberJack.Quest", "Embaucher un bucheron"},

            { "BuildGunPowder.Quest", "Fabriquez de la poudre à canon. Trouvez-la dans le menu des bâtiments de l'industrie"},
            { "ImportSulfur.Quest", "Au port, importer 1000 " + Unit.CurrentWeightUnitsString() + " de souffre"},
            { "GunPowderHire.Quest", "Embaucher un ouvrier dans le bâtiment de poudre à canon"},

            { "ImportPotassium.Quest", "Au port, importer 1000 " + Unit.CurrentWeightUnitsString() + " de potassium"},
            { "ImportCoal.Quest", "Au port, importer 1000 " + Unit.CurrentWeightUnitsString() + " de charbon"},

            { "ProduceGunPowder.Quest", "Produisons " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " de poudre à canon. Notez que vous aurez besoin de soufre, de potassium et de charbon pour produire de la poudre à canon"},
            { "ExportGunPowder.Quest", "Au port exporter 100 " + Unit.CurrentWeightUnitsString() + " de poudre à canon"},

            { "BuildLargeShack.Quest", "Construire une Grande Cabane et la population se développera plus rapidement"},

            { "BuildA2ndDock.Quest", "Construire un second port. Ce port ne peut être utilisé que pour les importations, vous pouvez importer des matières premières ici et les exporter à un autre port"},
            { "Rename2ndDock.Quest", "Renommez les ports, afin qu'ils ne soient utilisés que pour les importations et les exportations"},

            { "Import2000Wood.Quest", "Au port d'importations, importer 2000 " + Unit.CurrentWeightUnitsString() + " de bois. Cette matière première est nécessaire pour tout, car elle est utilisée comme combustible"},

            //IT HAS FINAL MESSAGE 
            //last quest it has a final message to the player. if new quest added please put the final message in the last quest
            { "Import2000Coal.Quest", "Au port d'importations, importer 2000 " + Unit.CurrentWeightUnitsString() + " de charbon. Le charbon aussi, est nécessaire pour tout parce qu'il est utilisé comme combustible. J'espère que vous avez apprécié l'expérience jusqu'ici. Continuez à agrandir votre colonie, et sa richesse. Aidez-nous à améliorer le jeu. Participez aux forums en ligne votre voix et vos opinions sont importantes! Amusez-vous Sugarmiller!"},

            //



            //Quest Titles
            { "Tutorial.Quest.Title", "Didacticiel"},
            { "Lamp.Quest.Title", "Lampadaire"},

            { "Shack.Quest.Title", "Construire une cabane"},
            { "SmallFarm.Quest.Title", "Construire une ferme agricole"},
            { "FarmHire.Quest.Title", "Embaucher 2 fermiers"},


            { "FarmProduce.Quest.Title", "Producteur agricole"},

            { "Export.Quest.Title", "Exportations"},
            { "HireDocker.Quest.Title", "Embaucher un docker"},
            { "MakeBucks.Quest.Title", "Gagner de l'argent"},
            { "HeavyLoad.Quest.Title", "Heavy Load"},
            { "HireHeavy.Quest.Title", "Embaucher un transporteur lourd"},

            { "ImportOil.Quest.Title", "Huile de baleine"},

            { "Population50.Quest.Title", "50 citoyens"},
            
            //
            { "Production.Quest.Title", "Fabriquer des armes"},
            { "ChangeProductToWeapon.Quest.Title", "Changer le produit"},
            { "BlackSmithHire.Quest.Title", "Embaucher 2 forgerons"},
            { "WeaponsProduce.Quest.Title", "Forger les armes"},
            { "ExportWeapons.Quest.Title", "Faire des bénéfices" },
            
            //
            { "BuildFishingHut.Quest.Title", "Construire une hutte de pêcheur"},
            { "HireFisher.Quest.Title", "Embaucher un pêcher"},
            { "BuildLumber.Quest.Title", "Construire une scierie"},
            { "HireLumberJack.Quest.Title", "Embaucher un bucheron"},
            { "BuildGunPowder.Quest.Title", "Fabriquer de la poudre à canon"},
            { "ImportSulfur.Quest.Title", "Importer du souffre"},
            { "GunPowderHire.Quest.Title", "Embaucher un travailleur de poudre à canon"},
            { "ImportPotassium.Quest.Title", "Importer du potassium"},
            { "ImportCoal.Quest.Title", "Importer du charbon"},
            { "ProduceGunPowder.Quest.Title", "Fabriquer de la poudre à canon"},
            { "ExportGunPowder.Quest.Title", "Exporter de la poudre à canon"},
            { "BuildLargeShack.Quest.Title", "Construrie une Grande cabane"},
            { "BuildA2ndDock.Quest.Title", "Construire un deuxième port"},
            { "Rename2ndDock.Quest.Title", "Renommer le deuxième port"},
            { "Import2000Wood.Quest.Title", "Importer du bois"},
            { "Import2000Coal.Quest.Title", "Importer du charbon"},




            {"Tutorial.Arrow", "Voici le didacticiel. Une fois terminée, vous gagnerez $10,000"},
            {"Quest.Arrow", "Voici le bouton de Quête. Vous pouvez accéder à la fenêtre Quête en cliquant dessus"},
            {"New.Quest.Avail", "Au moins une quête est disponible"},
            {"Quest_Button.HoverSmall", "Quête"},



            //Products
            //Notification.Init()
            {"RandomFoundryOutput", "Minerai fondu"},

            //OrderShow.ShowToSetCurrentProduct()
            { "RandomFoundryOutput (Ore, Wood)", "Minerai fondu (Minerai, Bois)"},



            //Bulleting helps
            {"Help.Bulletin/Prod/Produce", "Ici est montré ce qui est produit au village."},
            {"Help.Bulletin/Prod/Expire", "Ici est montré ce qui a expiré au village."},
            {"Help.Bulletin/Prod/Consume", "Ici est montré ce qui est consommé par votre peuple."},

            {"Help.Bulletin/Prod/Spec", "Dans cette fenêtre, vous pouvez voir les entrées nécessaires pour chaque produit, où cela se construit et le prix. "
            + "Faites défiler jusqu'au haut pour voir les en-têtes. Notez qu'un simple produit peut avoir plus d'une formule pour le fabriquer."},

            {"Help.Bulletin/General/Buildings", "Il s'agit d'un résumé du nombre de bâtiments de chaque type."},

            {"Help.Bulletin/General/Workers", "Dans cette fenêtre, vous pouvez affecter des travailleurs à des travaux dans différents bâtiments. "
            + "Pour un bâtiment permettre à plus de personnes dans le travail, doit être inférieure à la capacité et doit trouver au moins une personne sans-emploi."},

            {"Help.Bulletin/Finance/Ledger", "Voici votre comptabilité. Le salaire est le montant d'argent versé à un travailleur. Plus les gens travaillent, plus le salaire sera payé."},
            {"Help.Bulletin/Finance/Exports", "Une représentation des exportations"},
            {"Help.Bulletin/Finance/Imports", "Une représentation des importations"},


            { "Help.Bulletin/Finance/Prices", "...."},


            {"LoadWontFit", "Ce chargement ne rentre pas dans la zone de stockage"},

            {"Missing.Input", "Le bâtiment ne peut pas produire (les intrants doivent figurer dans le stockage du bâtiment). Entrées manquantes: \n" },





            //in game
            
            { "Buildings.Ready", "\n Bâtiments prêts à être construit:"},
            { "People.Living", "Personnes vivant dans cette maison:"},
            { "Occupied:", "Remplis:"},
            { "|| Capacity:", "|| Capacité:"},
            { "Users:", "\nUtilisateurs:"},
            { "Amt.Cant.Be.0", "Le montant ne peut pas être 0"},
            { "Prod.Not.Select", "Sélectionner un produit"},


            //articles
            { "The.Male", "Le"},
            { "The.Female", "La"},

            //
            { "Build.Destroy.Soon", "Ce bâtiment sera bientôt détruit. Si le stockage n'est pas vide, il doit être vidé par des brouettes"},




            //words
            //Field Farms
            { "Bean", "Haricot"},
            { "Potato", "Patate"},
            { "SugarCane", "Canne à sucre"},
            { "Corn", "Blé"},
            { "Cotton", "Coton"},
            { "Banana", "Banane"},
            { "Coconut", "Noix de coco"},
            //Animal Farm
            { "Chicken", "Poulet"},
            { "Egg", "Oeuf"},
            { "Pork", "Porc"},
            { "Beef", "Boeuf"},
            { "Leather", "Cuir"},
            { "Fish", "Poisson"},
            //mines
            { "Gold", "Or"},
            { "Stone", "Pierre"},
            { "Iron", "Fer"},

            // { "Clay", "Clay"},
            { "Ceramic", "Céramique"},
            { "Wood", "Bois"},

            //Prod
            { "Tool", "Outil"},
            { "Tonel", "Tonel"},
            { "Cigar", "Cigare"},
            { "Tile", "Tuile"},
            { "Fabric", "Tissu"},
            { "Paper", "Papier"},
            { "Map", "Carte"},
            { "Book", "Livre"},
            { "Sugar", "Sucre"},
            { "None", "Aucun"},
            //
            { "Person", "Personne"},
            { "Food", "Nourriture"},
            { "Dollar", "Dollar"},
            { "Salt", "Sel"},
            { "Coal", "Charbon"},
            { "Sulfur", "Souffre"},
            { "Potassium", "Potassium"},
            { "Silver", "Argent"},
            { "Henequen", "Henequen"},
            //
            { "Sail", "Voile"},
            { "String", "Corde"},
            { "Nail", "Clou"},
            { "CannonBall", "Boulet de canon"},
            { "TobaccoLeaf", "Sans tabac"},
            { "CoffeeBean", "Grain de café"},
            { "Cacao", "Cacao"},
            { "Weapon", "Arme"},
            { "WheelBarrow", "Brouette"},
            { "WhaleOil", "Huile de baleine"},
            //
            { "Diamond", "Diamand"},
            { "Jewel", "Bijou"},
            { "Rum", "Rhum"},
            { "Wine", "Vin"},
            { "Ore", "Minerai"},
            { "Crate", "Caisse"},
            { "Coin", "Pièce"},
            { "CannonPart", "Pièce de canon"},
            { "Steel", "Acier"},
            //
            { "CornFlower", "Fleur de lys"},
            { "Bread", "Pain"},
            { "Carrot", "carotte"},
            { "Tomato", "Tomate"},
            { "Cucumber", "Concombre"},
            { "Cabbage", "Choux-fleur"},
            { "Lettuce", "Salade"},
            { "SweetPotato", "patate douce"},
            { "Yucca", "Yucca"},
            { "Pineapple", "Ananas"},
            //
            { "Papaya", "Papaye"},
            { "Wool", "Laine"},
            { "Shoe", "Chaussure"},
            { "CigarBox", "Boîte de cigarre"},
            { "Water", "Eau"},
            { "Beer", "Bière"},
            { "Honey", "Miel"},
            { "Bucket", "Seau"},
            { "Cart", "Chariot"},
            { "RoofTile", "Rooftile"},
            { "FloorTile", "Dalle"},
            { "Furniture", "Mobilier"},
            { "Crockery", "Vaisselle"},

            { "Utensil", "Utensile"},
            { "Stop", "Arrêter"},


            //more Main GUI
            { "Workers distribution", "Répartition des travailleurs"},
            { "Buildings", "Bâtiments"},

            { "Age", "Age"},
            { "Gender", "Genre"},
            { "Height", "Taille"},
            { "Weight", "Poids"},
            { "Calories", "Calories"},
            { "Nutrition", "Alimentation"},
            { "Profession", "Profession"},
            { "Spouse", "Conjoint"},
            { "Happinness", "Bonheur"},
            { "Years Of School", "Années d'école"},
            { "Age majority reach", "Age de la majorité atteint"},
            { "Home", "Maison"},
            { "Work", "Travail"},
            { "Food Source", "Source alimentaire"},
            { "Religion", "Religion"},
            { "Chill", "Chilll"},
            { "Thirst", "Soif"},
            { "Account", "Compte"},

            { "Early Access Build", "Développement en accès anticipé"},

            //Main Menu
            { "Resume Game", "Reprendre la partie"},
            { "Continue Game", "Continuer la partie"},
            { "Tutorial(Beta)", "Didacticiel"},
            { "New Game", "Nouvelle partie"},
            { "Load Game", "Charger une partie"},
            { "Save Game", "Sauvegarder la partie"},
            { "Achievements", "Réussites"},
            { "Options", "Options"},
            { "Exit", "Quitter"},
            //Screens
            //New Game
            { "Town Name:", "Nom de la ville:"},
            { "Difficulty:", "Difficulté:"},
            { "Easy", "Facile"},
            { "Moderate", "Moyen"},
            { "Hard", "Difficile"},
            { "Type of game:", "Type de jeu:"},
            { "Freewill", "Libre arbitre"},
            { "Traditional", "Traditionnel"},
            { "New.Game.Pirates", "Pirates (Si coché, la ville pourrait subir des attaques de Pirates"},
            { "New.Game.Expires", "Péremption des aliments (Si coché, les aliments se périment)"},
            { "OK", "OK"},
            { "Cancel", "Annuler"},
            { "Delete", "Supprimer"},
            { "Enter name...", "Saisir un nom..."},
            //Options
            { "General", "Général"},
            { "Unit System:", "Unité de mesure:"},
            { "Metric", "Métrique"},
            { "Imperial", "Impérial"},
            { "AutoSave Frec:", "Sauvegarde Auto:"},
            { "20 min", "20 min"},
            { "15 min", "15 min"},
            { "10 min", "10 min"},
            { "5 min", "5 min"},
            { "Language:", "Langage:"},
            { "English", "Anglais"},
            { "Camera Sensitivity:", "Sensibilité de la Caméra:"},
            { "Themes", "Thèmes"},
            { "Halloween:", "Halloween:"},
            { "Christmas:", "Noël:"},
            { "Options.Change.Theme", "Relancer le jeu pour appliquer les changements"},

            { "Screen", "Ecran"},
            { "Quality:", "Qualité:"},
            { "Beautiful", "Magnifique"},
            { "Fantastic", "Fantastique"},
            { "Simple", "Simple"},
            { "Good", "Beau"},
            { "Resolution:", "Résolution:"},
            { "FullScreen:", "Plein écran:"},

            { "Audio", "Audio"},
            { "Music:", "Musique:"},
            { "Sound:", "Son:"},
            { "Newborn", "Nouveau-né"},
            { "Build Completed", "Construction terminée"},
            { "People's Voice", "Voix Peuple"},
            
            //in game gui
            { "Prod", "Prod"},
            { "Finance", "Finance"},



            //After Oct 20th 2019
            { "Resources", "Ressources"},
            { "Dollars", "Dollars"},
            { "Coming.Soon", "This building is coming soon to the game"},
            { "Max.Population", "Can't build more houses. Max population reached"},

            { "To.Unlock", "Déverouiller: "},
            { "People", "Peuple"},
            { "Of.Food", " de nourriture. "},//"of food. "
            { "Port.Reputation.Least", "Port réputation au moins à "},//"Port reputation at least at "
            { "Pirate.Threat.Less", "La menace pirate moins que "},//"Pirate threat less than "
            { "Skip", "Sauter"},

            //After Dec 8, 2018
            { "ReloadMod.HoverSmall", "Recharger des fichiers mod" },

    };

    }

    internal static void Clear()
    {
        _french.Clear();
    }

    internal static Dictionary<string, string> Dictionary()
    {
        return _french;
    }
    
    public static bool ContainsKey(string key)
    {
        return _french.ContainsKey(key);
    }

}