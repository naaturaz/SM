using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Spanish
{
    private static LangDict _sp = new LangDict();

    /// <summary>
    /// Will be called when a System Units is changed 
    /// </summary>
    public static void ReloadDict()
    {
        //ESPANNOL
        string _houseTailES = ". A los Azucareros les encanta comerse una buena comida de vez en cuando";
        string _animalFarmTailES = ", aquí se pueden criar diferentes animales";
        string _fieldFarmTailES = ", aquí se puede cultivar diferentes cultivos";
        string _asLongHasInputES = ", siempre y cuando tenga la materia prima necesaria";
        string _produceES = "Aquí los trabajadores producirán el producto selectionado, siempre y cuando exista la materia prima";
        string _storageES =
            "Aquí se almacenan todos los productos, si se llena los ciudadanos no tendrán donde almacenar sus cosas";
        string _militarES = "Con esta construcción la Amenaza Pirata decrece, " +
                                            "para ser efectiva necesita trabajadores. Mientras mas, mejor";

        _sp = new LangDict();

        //Descriptions
        //Infr
        _sp.Add("Road.Desc", "Solo para propósitos de decoración. Las personas se sienten mas felices si la via esta pavimentada alrededor de ellos");
        _sp.Add("BridgeTrail.Desc", "Por aquí las personas pasan de un lado del mapa a otro");
        _sp.Add("BridgeRoad.Desc", "Por aquí las personas pasan de un lado del mapa a otro. Los ciudadanos adoran estos puentes. " +
                               "Les da un sentido de prosperidad y felicidad" + _houseTailES);
        _sp.Add("LightHouse.Desc","Ayuda a que el puedo sea descubierto mas fácil. Añade a la Reputación del Puerto siempre y cuando la llama este encendida");
        _sp.Add(H.Masonry + ".Desc","Una construcción muy importante. Estos trabajadores son los que construyen y cuando no tienen nada que hacer se dedican a transportar bienes");
        _sp.Add(H.HeavyLoad + ".Desc","Una construcción muy importante. Estos trabajadores son los que construyen y cuando no tienen nada que hacer se dedican a transportar bienes");
        _sp.Add(H.StandLamp + ".Desc","Alumbra por las noches si hay Aceite de Ballena en la almacén.");

        //House
        _sp.Add("Bohio.Desc", "El Bohío, una casa con condiciones muy rudimentarias, los ciudadanos se abochornan de vivir aquí, una familia puede tener el máximo de 1 niño aquí" + _houseTailES);

        _sp.Add("Shack.Desc", "Casucha: Con condiciones de vida primitiva, las personas no son felices viviendo aquí y pueden tener un máximo de 2 niños");
        _sp.Add("MediumShack.Desc", "Casucha mediana: Las condiciones son básicas, y las personas sienten muy poca felicidad viviendo aquí y pueden tener un máximo de 2-3 niños");
        _sp.Add("LargeShack.Desc", "Casucha grande: Las condiciones son un poco mejor que básicas, y las personas sienten algo de felicidad viviendo aquí y pueden tener un máximo de 2-4 niños");

        _sp.Add("WoodHouseA.Desc", "Casa de madera mediana, una familia puede tener un máximo de 2-3 niños");
        _sp.Add("WoodHouseB.Desc", "Casa de madera grande, una familia puede tener un máximo de 3-4 niños");
        _sp.Add("WoodHouseC.Desc", "Casa de madera de lujo, una familia puede tener un máximo de 2-3 niños");

        _sp.Add("BrickHouseA.Desc","Casa de ladrillos mediana:, una familia puede tener el máximo de 3 a 4 niños aquí"+_houseTailES);
        _sp.Add("BrickHouseB.Desc","Casa de ladrillos de lujo:, una familia puede tener el máximo de 3 a 4 niños aquí"+_houseTailES);
        _sp.Add("BrickHouseC.Desc","Casa de ladrillos grande:, una familia puede tener el máximo de 4 niños aquí"+_houseTailES);

		//Farms
		//Animal
        _sp.Add("AnimalFarmSmall.Desc","Finca de animales chica"+_animalFarmTailES);
        _sp.Add("AnimalFarmMed.Desc","Finca de animales mediana"+_animalFarmTailES);
        _sp.Add("AnimalFarmLarge.Desc","Finca de animales grande"+_animalFarmTailES);
        _sp.Add("AnimalFarmXLarge.Desc","Finca de animales super grande"+_animalFarmTailES);
		//Fields
        _sp.Add("FieldFarmSmall.Desc","Finca de cultivos chica"+_fieldFarmTailES);
        _sp.Add("FieldFarmMed.Desc","Finca de cultivos mediana"+_fieldFarmTailES);
        _sp.Add("FieldFarmLarge.Desc","Finca de cultivos grande"+_fieldFarmTailES);
        _sp.Add("FieldFarmXLarge.Desc","Finca de cultivos super grande"+_fieldFarmTailES);
        _sp.Add(H.FishingHut + ".Desc","Aquí se pesca");

		//Raw
        _sp.Add("Clay.Desc","Aquí se produce barro, necesaria para construir ladrillos y otros productos mas");
        _sp.Add("Pottery.Desc","Aquí se producen productos de ceramica como platos, jarras, etc");
        _sp.Add("Mine.Desc","Esta es una mina");
        _sp.Add("MountainMine.Desc","Esta es una mina");
        _sp.Add("Resin.Desc","La Resina de saca de los arboles aquí");
        _sp.Add( H.LumberMill +".Desc","Aquí los trabajadores buscan y extraen recursos naturales como madera, piedra y minerales");
        _sp.Add("BlackSmith.Desc","Aquí el trabajador produce elementos de la forja "+_asLongHasInputES);
        _sp.Add("ShoreMine.Desc","Aquí se produce la sal, o arena");
        _sp.Add("QuickLime.Desc","Aquí los trabajadores producen cal");
        _sp.Add("Mortar.Desc","Aquí los trabajadores producen mezcla");

		//Prod
        _sp.Add("Brick.Desc",_produceES);
        _sp.Add("Carpentry.Desc",_produceES);
        _sp.Add("Cigars.Desc",_produceES);
        _sp.Add("Mill.Desc",_produceES);
        _sp.Add(H.Tailor+".Desc",_produceES);
        _sp.Add("Tilery.Desc",_produceES);
        _sp.Add("Armory.Desc",_produceES);
        _sp.Add(H.Distillery+".Desc",_produceES);
        _sp.Add("Chocolate.Desc",_produceES);
        _sp.Add("Ink.Desc",_produceES);

		//Ind
        _sp.Add("Cloth.Desc",_produceES);
        _sp.Add("GunPowder.Desc",_produceES);
        _sp.Add("PaperMill.Desc",_produceES);
        _sp.Add("Printer.Desc",_produceES);
        _sp.Add("CoinStamp.Desc",_produceES);
        _sp.Add("Silk.Desc",_produceES);
        _sp.Add("SugarMill.Desc",_produceES);
        _sp.Add("Foundry.Desc",_produceES);
        _sp.Add("SteelFoundry.Desc",_produceES);
        _sp.Add("SugarShop.Desc", "Produce derivados de la azúcar. " + _produceES);

		//trade
        _sp.Add("Dock.Desc","Aquí se pueden importar y exportar bienes");
        _sp.Add(H.Shipyard + ".Desc","Aquí se reparan los barcos, para ser efectivo debe tener los materiales necesarios");
        _sp.Add("Supplier.Desc","Aquí se abastecen los barcos para sus largos viajes, siempre y cuando tenga bienes aquí");
        _sp.Add("StorageSmall.Desc",_storageES);
        _sp.Add("StorageMed.Desc",_storageES);
        _sp.Add("StorageBig.Desc",_storageES);
        _sp.Add("StorageBigTwoDoors.Desc",_storageES);
        _sp.Add("StorageExtraBig.Desc",_storageES);

		//gov
        _sp.Add("Library.Desc","Aquí la gente viene a nutrirse de conocimiento. Mientras mas libros tenga es mejor");
        _sp.Add("School.Desc","Aquí empieza la educación de los Azucareros, mientras mas mejor");
        _sp.Add("TradesSchool.Desc","Aquí los Azucareros aprenden habilidades especiales, mientras mas mejor");
        _sp.Add("TownHouse.Desc","La casa de gobierno le da alegría y sentido de prosperidad a los ciudadanos");

		//other
        _sp.Add("Church.Desc","La iglesia le da felicidad y esperanza a la gente");
        _sp.Add("Tavern.Desc","Aquí la gente viene a tomar y a divertirse");

		//Militar
        _sp.Add("PostGuard.Desc",_militarES);
        _sp.Add("Fort.Desc",_militarES);
        _sp.Add("Morro.Desc",_militarES+". Una vez construida esta construcción los piratas te respetaran infinitamente");
        _sp.Add("WoodPost.Desc", "Ellos ven los pirates y bandidos primero de esta manera te puedes preparar mejor y con mas tiempo");


		//Structures Categores
        _sp.Add("Infrastructure", "Infrastructura");
        _sp.Add("Housing", "Casas");
        _sp.Add("Farming", "Comida");
        _sp.Add("Raw", "Materias Primas");
        _sp.Add("Production", "Produccion");
        _sp.Add("Industry", "Industrias");
        _sp.Add("Trade", "Comercio");
        _sp.Add("GovServices", "Servicios de Gobierno");
        _sp.Add("Other", "Otros");
        _sp.Add("Militar", "Militar");

		//Buildings name
		//Infr
        _sp.Add("StandLamp", "Lampara de Calle");
        _sp.Add("Trail", "Sendero");
        _sp.Add("Road", "Calle");
        _sp.Add("BridgeTrail", "Puente Pequeño");
        _sp.Add("BridgeRoad", "Puente Mediano");
        _sp.Add("CoachMan", "CoachMan");
        _sp.Add("LightHouse", "Faro");
        //_sp.Add("WheelBarrow", "Carretilleros");
        _sp.Add("StockPile", "Explanada");
        _sp.Add("Masonry", "Casa de Albañiles");
        _sp.Add("HeavyLoad", "Cocheros");

		//House
        _sp.Add("Shack", "Casucha");
        _sp.Add("MediumShack", "Casucha Mediana");
        _sp.Add("LargeShack", "Casucha Grande");

        _sp.Add("WoodHouseA", "Casa de Madera Mediana");
        _sp.Add("WoodHouseB", "Casa de Madera Grande" );
        _sp.Add("WoodHouseC", "Casa de Madera de Lujo");
        _sp.Add("BrickHouseA", "Casa de Ladrillos Mediana");
        _sp.Add("BrickHouseB", "Casa de Ladrillos de Lujo");
        _sp.Add("BrickHouseC", "Casa de Ladrillos Grande");

		//Farms
		//Animal
        _sp.Add("AnimalFarmSmall","Granja Pequeña de Animales");
        _sp.Add("AnimalFarmMed","Granja Mediana de Animales");
        _sp.Add("AnimalFarmLarge","Granja Grande de Animales");
        _sp.Add("AnimalFarmXLarge","Granja Enorme de Animales");
		//Fields
        _sp.Add("FieldFarmSmall","Granja Pequeña de Cultivos");
        _sp.Add("FieldFarmMed","Granja Mediana de Cultivos");
        _sp.Add("FieldFarmLarge","Granja Grande de Cultivos");
        _sp.Add("FieldFarmXLarge","Granja Enorme de Cultivos");
        _sp.Add("FishingHut", "Pescadores");

		//Raw
        _sp.Add("c","Mortero");
        _sp.Add("Pottery","Taller de Porcelana");
        _sp.Add("MountainMine","Mina");
        _sp.Add( "LumberMill" ,"Casa de Leñadores");
        _sp.Add("BlackSmith","Herrero");
        _sp.Add("ShoreMine","Mina marina");
        _sp.Add("QuickLime","Cal");

        //Prod
        _sp.Add("Brick","Horno de Ladrillos");
        _sp.Add("Carpentry","Carpinteria");
        _sp.Add("Cigars","Tabaqueria");
        _sp.Add("Mill","Molino");
        _sp.Add("Tailor","Sastre");
        _sp.Add("Tilery","Azulejos");
        _sp.Add("Armory","Fabrica de Armas");
        _sp.Add("Distillery","Destileria");
        //_sp.Add("Chocolate","Casa del Chocolate");
        //_sp.Add("Ink","Tinta");
		//Ind

        //_sp.Add("Cloth","Telar");
		//_sp.Add("GunPowder","Fabrica de Polvora");
        _sp.Add("PaperMill","Fábrica de Papel");
        _sp.Add("Printer","Imprenta");
        _sp.Add("CoinStamp","Casa de la Moneda");
        _sp.Add("SugarMill","Central Azucarero");
        _sp.Add("Foundry","Fundición");
        _sp.Add("SteelFoundry","Fundición de Acero");
        _sp.Add("SugarShop", "Casa del Azúcar");

        //trade
        _sp.Add("Dock","Puerto");
        _sp.Add("Shipyard","Astillero");
        _sp.Add("Supplier","Suministrador");
        _sp.Add("StorageSmall","Almacén Pequeña");
        _sp.Add("StorageMed","Almacén Mediana");
        _sp.Add("StorageBig","Almacén Grande");

		//gov
        _sp.Add("Library","Biblioteca");
        _sp.Add("School","Escuela");
        _sp.Add("TradesSchool","Escuela de Oficios");
        _sp.Add("TownHouse","Ayuntamiento");

		//other
        _sp.Add("Church","Iglesia");
        _sp.Add("Tavern","Taberna");

		//Militar
        _sp.Add("WoodPost", "Torre de Madera");
        _sp.Add("PostGuard","Torreón");
        _sp.Add("Fort","Fuerte");
        _sp.Add("Morro","Morro");

		//Decorations 
        _sp.Add("Fountain", "Fuente");
        _sp.Add("WideFountain","Fuente Grande");
        _sp.Add("PalmTree","Palma");
        _sp.Add("FloorFountain","Fuente Rasa");
        _sp.Add("FlowerPot","Flores");
        _sp.Add("PradoLion","León del Prado");

		//Main GUI
        _sp.Add("SaveGame.Dialog", "Salva tu partida");
        _sp.Add("LoadGame.Dialog", "Carga una partida");
        _sp.Add("NameToSave", "Salva tu partida como:");
        _sp.Add("NameToLoad", "La partida selecciona es:");
        _sp.Add("OverWrite", "Ya existe un archivo con este nombre. Quieres sobre escribirlo?" );
        _sp.Add("DeleteDialog", "?Estas seguro que quieres borrar esta partida?");
        _sp.Add("NotHDDSpace", "No hay espacio suficiente en torre {0} para salvar la partida");
        _sp.Add("GameOverPirate", "Lo siento, perdiste el juego! Los piratas te atacaron y mataron a todos.");
        _sp.Add("GameOverMoney", "Lo siento, perdiste el juego! La corona no te ayudara más con tu sueño Caribeño.");
        _sp.Add("BuyRegion.WithMoney", "Estas seguro que quieres comprar esta región.");
        _sp.Add("BuyRegion.WithOutMoney", "No tienes dinero para comprar esto ahora.");
        _sp.Add("Feedback", "Sugerencias si...:) Gracias. 8) ");
        _sp.Add("BugReport", "Bug, mandalo, gracias");
        _sp.Add("Invitation", "Pon el email de un amigo, quizás sea invitado a la Beta");
        _sp.Add("Info", "");//use for informational Dialogs

		//MainMenu
        _sp.Add("Types_Explain", "Tradicional: \nEn este juego algunas construcciones están  " +
                           "Bloqueadas al principio y tienes que desbloquearlas. " +
               "Lo bueno es que así tienes alguna manera de guiarte." +
               "\n\nLibre: \nTodas las construcciones están disponibles. " +
               "Lo malo es que puedes perder el juego más fácilmente.");

		//Tooltips 
		//Small Tooltips 
        _sp.Add("Person.HoverSmall", "Pers./Adul./Niñ.");
        _sp.Add("Emigrate.HoverSmall", "Emigrados");
        _sp.Add("Lazy.HoverSmall", "Desempleados");
        _sp.Add("Food.HoverSmall", "Comida");
        _sp.Add("Happy.HoverSmall", "Felicidad");
        _sp.Add("PortReputation.HoverSmall", "Reputación Portuaria");
        _sp.Add("Dollars.HoverSmall", "Dinero");
        _sp.Add("PirateThreat.HoverSmall", "Amenaza Pirata");
        _sp.Add("Date.HoverSmall", "Fecha (Mmm/A)");
        _sp.Add("MoreSpeed.HoverSmall", "Mas velocidad");
        _sp.Add("LessSpeed.HoverSmall", "Menos velocidad");
        _sp.Add("PauseSpeed.HoverSmall", "Pausa");
        _sp.Add("CurrSpeedBack.HoverSmall", "Velocidad actual");
        _sp.Add("ShowNoti.HoverSmall", "Notificaciones");
        _sp.Add("Menu.HoverSmall", "Menu Principal");
        _sp.Add("QuickSave.HoverSmall", "Salva rápida [F]");
        _sp.Add("Bug Report.HoverSmall", "Reporte un bug");
        _sp.Add("Feedback.HoverSmall", "Sugerencias");
        _sp.Add("Hide.HoverSmall", "Esconder");
        _sp.Add("CleanAll.HoverSmall", "Limpiar");
        _sp.Add("Bulletin.HoverSmall", "Control/Boletín");
        _sp.Add( "ShowAgainTuto.HoverSmall","Tutorial");
        _sp.Add("Quest_Button.HoverSmall", "Tareas");
        _sp.Add("TownTile.HoverSmall", "Nombre del pueblo");

        _sp.Add("More.HoverSmall", "Más");
        _sp.Add("Less.HoverSmall", "Menos");

        _sp.Add("BuyRegion.HoverSmall", "Compra región");
        _sp.Add("BullDozer.HoverSmall", "Bulldozer");
        _sp.Add("Help.HoverSmall", "Ayuda");

		//down bar
        _sp.Add("Infrastructure.HoverSmall", "Infraestructuras");
        _sp.Add("House.HoverSmall", "Casas");
        _sp.Add("Farming.HoverSmall", "Fincas");
        _sp.Add("Raw.HoverSmall", "Básico");
        _sp.Add("Prod.HoverSmall", "Producción");
        _sp.Add("Ind.HoverSmall", "Industrias");
        _sp.Add("Trade.HoverSmall", "Comercio");
        _sp.Add("Gov.HoverSmall", "Gobierno");
        _sp.Add("Other.HoverSmall", "Otros");
        _sp.Add("Militar.HoverSmall", "Militar");
        _sp.Add("WhereIsTown.HoverSmall", "Centrar el pueblo [P]");
        _sp.Add("WhereIsSea.HoverSmall", "Muestre/Esconda al mar");
        _sp.Add("Helper.HoverSmall", "Mini-Ayuda");
        _sp.Add("Tempeture.HoverSmall", "Temperatura");

		//building window
        _sp.Add("Gen_Btn.HoverSmall", "General");
        _sp.Add("Inv_Btn.HoverSmall", "Inventario");
        _sp.Add("Upg_Btn.HoverSmall", "Mejoras");
        _sp.Add("Prd_Btn.HoverSmall", "Producción");
        _sp.Add("Sta_Btn.HoverSmall", "Estadísticas");
        _sp.Add("Ord_Btn.HoverSmall", "Ordenes");
        _sp.Add("Stop_Production.HoverSmall", "Parar producción");
        _sp.Add("Demolish_Btn.HoverSmall", "Demoler");
        _sp.Add("More Salary.HoverSmall", "Pagar mas");
        _sp.Add("Less Salary.HoverSmall", "Pagar menos");
        _sp.Add("Next_Stage_Btn.HoverSmall", "Compre");
        _sp.Add("Current_Salary.HoverSmall", "Salario actual");
        _sp.Add("Current_Positions.HoverSmall", "Posiciones actuales");
        _sp.Add("Add_Import_Btn.HoverSmall", "Añade una importación");
        _sp.Add("Add_Export_Btn.HoverSmall", "Añade una exportación");
        _sp.Add("Upg_Cap_Btn.HoverSmall", "Mejora la capacidad");
        _sp.Add("Close_Btn.HoverSmall", "Cerrar");
        _sp.Add("ShowPath.HoverSmall", "Enseñar camino");
        _sp.Add("ShowLocation.HoverSmall", "Enseñar lugar");
        _sp.Add("Max_Positions.HoverSmall", "Max de trabajadores");
        _sp.Add( "Rate.HoverSmall", "Por favor, evaluá el juego");

		//addOrder windiw
        _sp.Add("Amt_Tip.HoverSmall", "Cantidad de productos");

		//Med Tooltips 
        _sp.Add("Build.HoverMed", "Situar construcción: 'Click izquierdo' \n" +
                               "Rotar construcción: tecla 'R' \n " +
                               "Cancelar: 'Click derecho'");
        _sp.Add("Current_Salary.HoverMed", "Los trabajadores prefieren trabajar donde se pague mas dinero." +
                                           " Si dos lugares pagan igual entonces escogerán el que este mas cerca a" +
                                           " casa.");

		//Notifications
        _sp.Add("BabyBorn.Noti.Name", "Recién nacido");
        _sp.Add("BabyBorn.Noti.Desc", "Un niño a nacido que alegría");
        _sp.Add("PirateUp.Noti.Name", "Los piratas se acercan");
        _sp.Add("PirateUp.Noti.Desc", "Un barco de bandera pirata se ha visto cerca de la costa");
        _sp.Add("PirateDown.Noti.Name", "Los piratas te temen");
        _sp.Add("PirateDown.Noti.Desc", "Hoy los piratas te respetan un poco mas");

        _sp.Add("Emigrate.Noti.Name", "Una persona a emigrado");
        _sp.Add("Emigrate.Noti.Desc", "Las personas emigran cuando no están felices");
        _sp.Add("PortUp.Noti.Name", "El puerto de conoce");
        _sp.Add("PortUp.Noti.Desc", "Tu puerto esta recibiendo mas atención por los comerciantes");
        _sp.Add("PortDown.Noti.Name", "Tu puerto es desconocido");
        _sp.Add("PortDown.Noti.Desc", "Tu puerto se conoce cada vez menos entre los comerciantes");

        _sp.Add("BoughtLand.Noti.Name", "Nuevo lote de tierra");
        _sp.Add("BoughtLand.Noti.Desc", "Nuevo lote de tierra ha sido comprado");

        _sp.Add("ShipPayed.Noti.Name", "Pago de comercio");
        _sp.Add("ShipPayed.Noti.Desc", "Un barco a pagado por los bienes adquiridos en tu puerto");
        _sp.Add("ShipArrived.Noti.Name", "Barco ha llegado");
        _sp.Add("ShipArrived.Noti.Desc", "Un barco ha llegado a una de nuestras construcciónes marítimas");

        _sp.Add("AgeMajor.Noti.Name", "Un Trabajador Nuevo");
        _sp.Add("AgeMajor.Noti.Desc", "{0} esta listo(a) para trabajar");


        _sp.Add("PersonDie.Noti.Name", "Una persona ha muerto");
        _sp.Add("PersonDie.Noti.Desc", "{0} ha muerto");

        _sp.Add("DieReplacementFound.Noti.Name", "Una persona ha muerto");
        _sp.Add("DieReplacementFound.Noti.Desc", "{0} ha muerto y ha sido reemplazado en su trabajo");

        _sp.Add("DieReplacementNotFound.Noti.Name", "Una persona ha muerto");
        _sp.Add("DieReplacementNotFound.Noti.Desc", "{0} ha muerto. No se encontró reemplazo en su trabajo");


        _sp.Add("FullStore.Noti.Name", "Una almacén se esta llenando");
        _sp.Add("FullStore.Noti.Desc", "Una almacén esta al {0}% de su capacidad");

        _sp.Add("CantProduceBzFullStore.Noti.Name", "El trabajador no puede producir");
        _sp.Add("CantProduceBzFullStore.Noti.Desc", "{0} el trabajador no puede producir porque su almacén esta llena");

        _sp.Add("NoInput.Noti.Name", "Al menos un insumo falta en el edificio");
        _sp.Add("NoInput.Noti.Desc", "Un edificio no produce {0} porque le falta aunque sea un insumo");

        _sp.Add("Built.Noti.Name", "Una construcción ha sido terminada");
        _sp.Add("Built.Noti.Desc", "{0} a sido construido(a)");

        _sp.Add("cannot produce", "La producción se ha parado");






		//Main notificaion
		//Shows on the middle of the screen
        _sp.Add("NotScaledOnFloor", "La construcción esta muy cerca al mar o una montaña");
        _sp.Add("NotEven", "El piso no esta parejo en la base de la construcción");
        _sp.Add("Colliding", "La construcción choca con otra");
        _sp.Add("BadWaterHeight", "La construcción esta muy alta o muy baja en el agua");
        _sp.Add("LockedRegion", "Necesitas ser dueño de esta tierra para construir aquí");
        _sp.Add("HomeLess", "La gente en esta casa no tiene a donde ir. Por favor construye una" +
                           " nueva casa que pueda albergar a esta familia");
        _sp.Add("LastFood", "No puedes destruir la única almacén en la villa");
        _sp.Add("LastMasonry", "No puedes destruir la única casa de albañiles en la villa");


		//Mini help
        _sp.Add("Camera", "Camara: Use [AWSD] or el cursor para mover. " +
                       "Presione el botón del medio del ratón para rotar cámara, o [Q] y [E]");
        _sp.Add("SeaPath", "Presione en el botón 'Mostrar al mar' " +
                           "y el camino mas cercano al mar sera mostrado");
        _sp.Add("Region", "Region: Necesitas ser dueño de una región para construir en ella. " +
                       "Presione en la señal 'Se vende' para comprar una");
        _sp.Add("PeopleRange", "Rango: El circulo azul gigante es el rango de cada construcción");

        // _sp.Add("PirateThreat.Help", "Amenaza Pirata: Esto es cuan al dia están los piratas con tu puerto. " +
        //                            "Se incrementa a medida que acumules mas dinero y riquezas. " +
        //                            "Si pasa 90 entonces pierdes el juego.");

        // _sp.Add("PortReputation.Help", "Reputación Portuaria: Mientras mas comerciantes y marineros conozcan tu puerto mas lo visitaran." +
        //                                    "Si quieres aumentar esto asegurate de que siempre tenga ordenes en tus construcciones marítimas" +
        //                                " (Puerto, Astillero, Abastecedor)");
        // _sp.Add("Emigrate.Help", "Emigrados: Cuando la gente esta infeliz por algunos años se van de tus tierras. " +
        //                        "Lo malo es que no viraran, producirán bienes o tendrán niños jamas." +
        //                            "Lo bueno es que aumentan 'La Reputación Portuaria'");
        // _sp.Add("Food.Help", "Comida: Mientras mas variedad de comidas las personas tengan mas felices serán.");

        // _sp.Add("Weight.Help", "Peso: Todos los pesos en el juego están en Kg o Lbs" +
        //                        " dependiendo en el sistema de unidad seleccionado." +
        //                        " Se puede cambiar en 'Opciones' en el 'Menu Principal'");



        // _sp.Add("More.Help", "Si necesita mas ayuda siempre es una buena idea pasar el tutorial, or o postear una pregunta en el Forum");

		// //more 
        // _sp.Add("Products Expiration.Help", "Caducidad de productos: Como en la vida real los productos expiran. En la tabla the productos expirados se puede ver si alguno ha expirado Bulletin/Prod/Expire");
        // _sp.Add("Horse Carriages.Help", "Las personas con carretillas tiene limites de carga. Por eso estas carretas con caballos son usadas en el juego, ya que pueden cargar mucho mas. Como resultado la economía se mueve mas de prisa. Una persona carga alrededor de 15KG, un carretillero 60KG, y las carretas chicas hasta 240KG. Construye un HeavyLoad para usarlas");
        // _sp.Add("Usage of goods.Help", "Consumo de bienes: Cajas, barriles, carretillas, carretas, herramientas, ropa, cerámicas, muebles y utensilios son todos necesarios para mantener las actividades de la villa. A medida que estos bienes son usados disminuye la cantidad en el almacén, por ej. una persona no cargara nada si no hay cajas");
        // _sp.Add("Happiness.Help", "Felicidad: La felicidad de las personas esta influenciada por varios factores. Variedad de comidas, satisfacción religiosa, esparcimiento, confort de la casa, nivel de educación, utensilios, cerámica y ropa.");
        // _sp.Add("Line production.Help", "Linea de producción: Para hacer un KG de puntillas tienes que encontrar y minar los minerales, en la fundición derretir el hierro, y finalmente en el herrero hacer las puntillas. O simplemente comprarla en el puerto");
        // _sp.Add("Bulletin.Help", "El icono con las paginas en la barra inferior es la ventana de Boletín/Control. Por favor toma un minuto para explorarla.");
        // _sp.Add("Trading.Help", "Necesitas al menos un puerto para comerciar. En el puerto puedes agregar ordenes de importación y exportación. Si necesitas mas ayuda puedes pasar el tutorial.");

        // _sp.Add("Combat Mode.Help", "Se activa cuando un pirata o bandido es visto por uno de tus ciudadanos.");

        // _sp.Add("Population.Help", "Cuando los jóvenes cumplen 16 años se mudan a una casa vaciá si existe. Si siempre hay casas vaciás el crecimiento de la población esta garantizado.");

        // _sp.Add("F1.Help", "Presiona [F1] para ayuda");

        // _sp.Add("Inputs.Help", "Si un edificio no produce porque le faltan insumos, chequea que los insumos necesarios estén en la almacén y que tengas trabajadores en la Casa De Albañiles");




        // _sp.Add("WheelBarrows.Help", "Los carretilleros son los trabajadores de la Casa de Albañiles. Si ellos no tienen nada que hacer entonces harán el trabajo de carretilleros. Si necesitas algún insumo en un edificio, asegurate de tener bastantes de estos trabajando y por su puesto los insumos disponibles en la almacén");









        _sp.Add("TutoOver", "Tu premio sera de $10,000 si es la 1era vez que completas el tutorial. Este es el fin del tutorial ahora puedes seguir jugando.");

		//Tuto
        _sp.Add("CamMov.Tuto", "El premio por completar el tutorial son $10,000 (solo un premio por juego). Paso 1: Usa [WASD] o las teclas del cursos para mover la cámara. Haz esto por al menos 5 segundos");
        _sp.Add("CamMov5x.Tuto", "Usa [WASD] o las teclas del cursos + 'Shift Izq' para mover la cámara 5 veces mas rápido. Haz esto por al menos 5 segundos");
        _sp.Add("CamRot.Tuto", "Presiona la rueda del ratón y después mueve el ratón para girar la cámara. Haz esto por al menos 5 segundos");

        _sp.Add("BackToTown.Tuto", "Presiona la tecla [P] para ir al centro del pueblo");

        _sp.Add("BuyRegion.Tuto", "Necesitas ser dueño de una región para poder construir en ella. Haz click en el signo de '+' en la barra inferior, después en la señal de 'For Sale' " +
                   " en una región para comprarla. Estas construcciones pueden ser construidas sin ser dueño de la región:" +
                   " (Puerto, Mina marítima, Faro, Torreón)"
           );

        _sp.Add("Trade.Tuto", "Eso fue fácil ahora viene lo difícil. Haz click en 'Comercio', en la barra inferior. "+
               "Cuando pases el cursor del ratón se vera que dice 'Comercio'");
        _sp.Add("CamHeaven.Tuto", "Gira la rueda del ratón hacia detrás hasta que alcances el límite en el cielo. Esta vista es usada para emplazar grandes construcciones como el 'Puerto'");

        _sp.Add("Dock.Tuto", "Haz click en la construcción 'Puerto'. Cuando pases el cursor del ratón por encima del icono saldrá su costo y descripción");
        _sp.Add("Dock.Placed.Tuto", "Ahora viene lo mas difícil. Puedes usar la tecla 'R' para rotar la construcción y click derecho para cancelar. "+
               " Esta construcción tiene una parte que va en tierra y otra en agua." +
               " La flecha debe ir en el agua, la sección del almacenaje en tierra. Cuando la flecha se ponga blanca haz click izq para emplazar el edificio.");

        _sp.Add("2XSpeed.Tuto", "Aumenta la velocidad del juego, en la parte superior de la pantalla en el centro, haz click en "
                   +" 'Mas' hasta que aparezca el '2x'");

        _sp.Add("ShowWorkersControl.Tuto", "Haz click en botón de 'Control/Boletín', en la parte inferior de la pantalla. "+
               "Si le pasas el cursor del ratón por encima se vera 'Control/Boletín'");
        _sp.Add("AddWorkers.Tuto", "Haz click en el signo de '+', Asi es como se añaden mas trabajadores.");


        _sp.Add("HideBulletin.Tuto", "En este formulario puedes controlar y ver varios aspectos de la partida. Haz click fuera del formulario o 'OK' para cerrarlo");
        _sp.Add("FinishDock.Tuto", "Ahora termina el Puerto. Mientras mas trabajadores tenga en la Casa de Albañiles mas rápido se terminara."
           + " Tambien asegurate que tienes todos los materiales necesarios para construirlo.");
        _sp.Add("ShowHelp.Tuto", "Haz click en el botón de ayuda, en la barra inferior. "+
               " Aquí puedes encontrar la ayuda del juego.");

        _sp.Add("SelectDock.Tuto", "Los barcos escogen bienes al azar del inventario del puerto. Necesitas trabajadores para que muevan los bienes hacia y desde el puerto para las almacenes. Estos trabajadores utilizan cajas y carretillas para mover los bienes. Ahora haz click en el Puerto.");
        _sp.Add("OrderTab.Tuto", "Haz click en las Ordenes en el formulario del Puerto");
        _sp.Add("ImportOrder.Tuto", "Haz click en el signo de '+' al lado de 'Orden de Importación'");



        _sp.Add("AddOrder.Tuto", "Ahora navega hasta el final de la lista y escoge 'Madera', pon la cantidad de 100. Después haz click en el botón de 'Añadir'");
        _sp.Add("CloseDockWindow.Tuto", "Ya la orden fue añadida. Un barco depositara estos bienes en el puerto. Después tus trabajadores portuarios lo llevaran para la almacen mas cercana. Ahora cierra este formulario.");
        _sp.Add("Rename.Tuto", "Haz click en una persona y después click en la barra de titulo del formulario. De esta manera le puedes cambiar el nombre a cualquier persona o edificio. Haz click afuera del titulo y los cambios serán guardados");
        _sp.Add("RenameBuild.Tuto", "Selecciona una construcción y cambiale el nombre de la misma manera.");

        _sp.Add("BullDozer.Tuto", "Selecciona el botón con el Bulldozer. Después elimina un árbol o roca del terreno.");


        _sp.Add("Budget.Tuto", "Haz click en el botón 'Control/Boletín', después en 'Finanzas' y después en 'Presupuesto'");
        _sp.Add("Prod.Tuto", "Haz click en el menu 'Prod' y después en 'Producido'. Muestra lo producido en los últimos 5 años");
        _sp.Add("Spec.Tuto", "Haz click en el menu 'Prod' después en 'Spec'. Aquí se ve exactamente como hacer todos los bienes en el juego. Los insumos necesarios, donde es producido y ademas el precio");
        _sp.Add("Exports.Tuto", "Haz click en el menu 'Finanzas' y después en 'Exportaciones'. Aquí se ve un sumario de las exportaciones");







		//Quest
        _sp.Add("Tutorial.Quest", "Desafió: Termina el tutorial. $10,000 en premio. Toma alrededor de 3 minutos para ser completado");
        _sp.Add("Lamp.Quest", "Desafió: Construye una farola. Esta en Infraestructuras, son encendidas de noche si hay Aceite de Ballena en la Almacén");
        _sp.Add("Shack.Quest", "Desafió: Construye una casucha. Estas son casas baratas. Cuando las personas cumplen 16 años se mudan a un casa nueva si existe. De esta manera se garantiza el crecimiento de la población. [F1] Ayuda");
        _sp.Add("SmallFarm.Quest", "Desafió: Construye una Finca de Cultivos Chica. Necesitas estas para alimentar a tu pueblo");
        _sp.Add("FarmHire.Quest", "Desafió: Contrata a dos granjeros en la Finca de Cultivos Chica. Haz click en la finca y después en el signo de mas para asignar trabajadores. Para esto necesitas tener trabajadores desempleados");
        _sp.Add("FarmProduce.Quest", "Desafió: Produce " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " de Frijol en la Finca de Cultivos Chica. Haz click en la pestaña 'Stat' y te mostrara la producción de los últimos 5 años. Puedes ver el avance en el desafió en el formulario de desafíos. Si construyes mas Fincas de Cultivos Chica ayudaran a pasar este desafio");
        _sp.Add("Transport.Quest", "Desafió: Transporta el Frijol de la Finca hacia la Almacén. Para hacer esto asegurate de que hay trabajadores en la Casa de Albañiles. Ellos se convierten en carretilleros cuando no trabajan");
        _sp.Add("Export.Quest", "Desafió: Exporta 300 " + Unit.CurrentWeightUnitsString() + " de Frijol. Añade una orden de Exportación en el Puerto. Si no tienes un Puerto entonces construye uno."+
               "El icono del Puerto esta en Comercio. Cuando este hecho haz click en la pestaña de ordenes, añade una orden de exportación, y selecciona el producto y la cantidad a exportar.");
        _sp.Add("HireDocker.Quest", "Desafió: Contrata un portuario. La única tarea de ellos es mover bienes desde el Almacén hacia el Puerto si estas exportando."+
               " O vice-versa si estas importando. Ellos trabajan cuando hay ordenes en el puerto y los bienes están listos para su transporte. Sino se quedan en casa descanzando." +
                   " Si ya tienes trabajadores aquí despidamos a todos y después contrata a uno de nuevo.");
        _sp.Add("MakeBucks.Quest", "Desafió: Haz $100 exportando bienes en el Puerto. "+
               "Cuando un barco llegue pagara bienes al azar que tenga en las bodegas de tu Puerto");
        _sp.Add("HeavyLoad.Quest", "Desafió: Construye el edificio de Carga Pesada. Estos son transportistas que cargan mas peso. Serán muy útiles cuando mucha carga necesita ser transportada en tu villa");
        _sp.Add("ImportOil.Quest", "Desafió: Importa 500 " + Unit.CurrentWeightUnitsString() + " de Aceite de Ballena en el Puerto. Es necesario para encender las Farolas por las noches.");
        _sp.Add("Population50.Quest", "Obtén 50 personas en total");


		//added Aug 11 2017, result: sep 9(30% off biggest sale ever)
        _sp.Add("Production.Quest", "Ahora vamos a producir armas que después venderemos. Primero construye un Herrero. Esta en el menu 'Básico'");




        _sp.Add("ChangeProductToWeapon.Quest", "En el Herrero(Blacksmith), click en la pestaña de 'Productos(Products)' y cambien la producción a 'Armas. Los trabajadores traeran los materiales necesarios si están disponibles");
        _sp.Add("BlackSmithHire.Quest", "Contrata a dos herreros");
        _sp.Add("WeaponsProduce.Quest", "Ahora produce " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " de Armas en el Herrero. Click en 'Stat', para que veas un reporte productivo de los ultimos 5 años. Puedes ver el avance del reto en la ventana 'Retos'.");
        _sp.Add("ExportWeapons.Quest", "Ahora exporta 100 " + Unit.CurrentWeightUnitsString() + " de Armas. En el Puerto añade una orden de exportación. Te darás cuenta que hacer Armas es un negocio con buen lucro");

		//added Sep 14 2017
        _sp.Add("BuildFishingHut.Quest", "Construye un pesquero. De esta manera los ciudadanos tienen variedad de comidas para comer y serán mas felices");
        _sp.Add("HireFisher.Quest", "Contrata a un pescador");

        _sp.Add("BuildLumber.Quest", "Construya una 'Casa de Leñadores(Lumbermill)'. Esta en el menu 'Raw'");
        _sp.Add("HireLumberJack.Quest", "Contrata a un leñador");

        _sp.Add("BuildGunPowder.Quest", "Construye una Fabrica de Pólvora. Esta en el menu de construcciones 'Industrias'");
        _sp.Add("ImportSulfur.Quest", "En el Puerto importa " + Unit.CurrentWeightUnitsString() + " de Azufre");
        _sp.Add("GunPowderHire.Quest", "Contrata a un trabajador en la Fabrica de Pólvora");

        _sp.Add("ImportPotassium.Quest", "En el Puerto importa " + Unit.CurrentWeightUnitsString() + " de Potasio");
        _sp.Add("ImportCoal.Quest", "En el Puerto importa " + Unit.CurrentWeightUnitsString() + " de Carbón");

        _sp.Add("ProduceGunPowder.Quest", "Produce ahora " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " de Pólvora. Necesitaras Azufre, Potasio y Carbón para producir Pólvora.");
        _sp.Add("ExportGunPowder.Quest", "En el Puerto exporta " + Unit.CurrentWeightUnitsString() + " de Pólvora.");

        _sp.Add("BuildLargeShack.Quest", "Construye una Casucha Grande, con casas mas grandes la población aumentara mas rápido.");

        _sp.Add("BuildA2ndDock.Quest", "Construye otro Puerto mas. Este puerto lo pudieses usar solo para importar, de esa manera importas materias primas y exportas bienes producidos en el otro Puerto");
        _sp.Add("Rename2ndDock.Quest", "Nombra los Puertos, asi recordaras cual usar para importar y exportar.");

        _sp.Add("Import2000Wood.Quest", "En el Puerto de Importaciones importa 2000 " + Unit.CurrentWeightUnitsString() + " de Madera. Esta materia prima se usa para todo porque se usa como combustible.");

		//IT HAS FINAL MESSAGE 
		//last quest it has a final message to the player. if new quest added please put the final message in the last quest
        _sp.Add("Import2000Coal.Quest", "En el Puerto de Importaciones importa 2000 " + Unit.CurrentWeightUnitsString() + " de Carbón. El Carbón también se puede usar como combustible. Espero estés disfrutando el juego hasta ahora. Sigue expandiendo tu colonia y riquezas. Por favor también ayuda a mejorar el juego. Participa en el forum de Steam y deja tus sugerencias y cualquier ideas que tengas para mejorar el juego. Diviértete!");
		//

        _sp.Add("CompleteQuest", "Tu premio es de ${0}");









		//Quest Titles
        _sp.Add("Tutorial.Quest.Title", "Tutorial");
        _sp.Add("Lamp.Quest.Title", "Lampara de Calle");

        _sp.Add("Shack.Quest.Title", "Construye una Casucha");
        _sp.Add("SmallFarm.Quest.Title", "Construye una Granja Pequeña");
        _sp.Add("FarmHire.Quest.Title", "Contrata dos Granjeros");


        _sp.Add("FarmProduce.Quest.Title", "Productor agrícola");

        _sp.Add("Export.Quest.Title", "Exportaciones");
        _sp.Add("HireDocker.Quest.Title", "Contratación portuaria");
        _sp.Add("MakeBucks.Quest.Title", "Haz dinero");
        _sp.Add("HeavyLoad.Quest.Title", "Carga pesada");
        _sp.Add("HireHeavy.Quest.Title", "Contrata a un cochero");

        _sp.Add("ImportOil.Quest.Title", "Aceite de ballena");

        _sp.Add("Population50.Quest.Title", "50 Ciudadanos");

		//
        _sp.Add("Production.Quest.Title", "Produce Armas");
        _sp.Add("ChangeProductToWeapon.Quest.Title", "Cambia el Producto");
        _sp.Add("BlackSmithHire.Quest.Title", "Contrata dos herreros");
        _sp.Add("WeaponsProduce.Quest.Title", "Produce Armas");
        _sp.Add("ExportWeapons.Quest.Title", "Ganancias");

		//
        _sp.Add("BuildFishingHut.Quest.Title", "Construye Casa de Pescadores");
        _sp.Add("HireFisher.Quest.Title", "Contrata a un Pescador");
        _sp.Add("BuildLumber.Quest.Title", "Construye Casa de Leñadores");
        _sp.Add("HireLumberJack.Quest.Title", "Contrata a un Leñador");
        _sp.Add("BuildGunPowder.Quest.Title", "Construye Fabrica de Pólvora");
        _sp.Add("ImportSulfur.Quest.Title", "Importa Azufre");
        _sp.Add("GunPowderHire.Quest.Title", "Contrata");
        _sp.Add("ImportPotassium.Quest.Title", "Importa Potasio");
        _sp.Add("ImportCoal.Quest.Title", "Importa Carbón");
        _sp.Add("ProduceGunPowder.Quest.Title", "Produce Pólvora");
        _sp.Add("ExportGunPowder.Quest.Title", "Exporta Pólvora");
        _sp.Add("BuildLargeShack.Quest.Title", "Construye una Casucha Grande");
        _sp.Add("BuildA2ndDock.Quest.Title", "Construye otro Puerto");
        _sp.Add("Rename2ndDock.Quest.Title", "Nombra el Puerto");
        _sp.Add("Import2000Wood.Quest.Title", "Importa Madera");
        _sp.Add("Import2000Coal.Quest.Title", "Importa Carbón");







		//in game
        _sp.Add("Buildings.Ready", "\n Edificios listos para ser construidos:");
        _sp.Add("People.Living", "Personas en esta casa:");
        _sp.Add("Occupied:", "En uso:");
        _sp.Add("|| Capacity:", "|| Capacidad:");
        _sp.Add("Users:", "\nUsuarios:");
        _sp.Add("Amt.Cant.Be.0", "La cantidad no puede ser cero.");
        _sp.Add("Prod.Not.Select", "Por favor seleccione un producto");

		//articles
        _sp.Add("The.Male", "El");
        _sp.Add("The.Female", "La");

		//
        _sp.Add("Build.Destroy.Soon", "Esta construcción sera destruida. Si el inventario no esta vació los carretilleros deberán hacerlo.");






		//words
		//Field Farms
        _sp.Add("Bean", "Frijol");
        _sp.Add("Potato", "Papa");
        _sp.Add("SugarCane", "Caña");
        _sp.Add("Corn", "Maíz");
        _sp.Add("Cotton", "Algodón");
        _sp.Add("Banana", "Plátano");
        _sp.Add("Coconut", "Coco");
		//Animal Farm
        _sp.Add("Chicken", "Pollo");
        _sp.Add("Egg", "Huevo");
        _sp.Add("Pork", "Cerdo");
        _sp.Add("Beef", "Res");
        _sp.Add("Leather", "Cuero");
        _sp.Add("Fish", "Pescado");
		//mines
        _sp.Add("Gold", "Oro");
        _sp.Add("Stone", "Piedra");
        _sp.Add("Iron", "Hierro");

        _sp.Add("Clay", "Arcilla");
        _sp.Add("Ceramic", "Cerámica");
        _sp.Add("Wood", "Madera");

		//Prod
        _sp.Add("Tool", "Herramienta");
        _sp.Add("Tonel", "Tonel");
        _sp.Add("Cigar", "Tabaco");
        _sp.Add("Tile", "Loza");
        _sp.Add("Fabric", "Tejido");
        //_sp.Add("GunPowder", "Pólvora");
        _sp.Add("Paper", "Papel");
        _sp.Add("Map", "Mapa");
        _sp.Add("Book", "Libro");
        _sp.Add("Sugar", "Azúcar");
        _sp.Add("None", "Ninguno");
		//
        _sp.Add("Person", "Persona");
        _sp.Add("Food", "Comida");
        _sp.Add("Dollar", "Peseta");
        _sp.Add("Salt", "Sal");
        _sp.Add("Coal", "Carbón");
        _sp.Add("Sulfur", "Sulfuro");
        _sp.Add("Potassium", "Potasio");
        _sp.Add("Silver", "Plata");
        _sp.Add("Henequen", "Henequén");
		//
        _sp.Add("Sail", "Vela");
        _sp.Add("String", "Cuerda");
        _sp.Add("Nail", "Puntilla");
        _sp.Add("CannonBall", "Bola de cañon");
        _sp.Add("TobaccoLeaf", "Hoja de tabaco");
        _sp.Add("CoffeeBean", "Grano de cafe");
        _sp.Add("Cacao", "Cocoa");
        _sp.Add("Chocolate", "Chocolate");
        _sp.Add("Weapon", "Arma");
        _sp.Add("WheelBarrow", "Carretilla");
		//
        _sp.Add("Diamond", "Diamante");
        _sp.Add("Jewel", "Joya");
        _sp.Add("Cloth", "Ropa");
        _sp.Add("Rum", "Ron");
        _sp.Add("Wine", "Vino");
        _sp.Add("Ore", "Mineral");
        _sp.Add("Crate", "Caja");
        _sp.Add("Coin", "Moneda");
        _sp.Add("CannonPart", "Pieza de cañon");
        _sp.Add("Ink", "Tinta");
        _sp.Add("Steel", "Acero");
		//
        _sp.Add("CornFlower", "Harina de castilla");
        _sp.Add("Bread", "Pan");
        _sp.Add("Carrot", "Zanahoria");
        _sp.Add("Tomato", "Tomate");
        _sp.Add("Cucumber", "Pepino");
        _sp.Add("Cabbage", "Col");
        _sp.Add("Lettuce", "Lechuga");
        _sp.Add("SweetPotato", "Boniato");
        _sp.Add("Yucca", "Yuca");
        _sp.Add("Pineapple", "Piña");
		//
        _sp.Add("Papaya", "Fruta bomba");
        _sp.Add("Wool", "Lana");
        _sp.Add("Shoe", "Zapato");
        _sp.Add("CigarBox", "Caja de tabaco");
        _sp.Add("Water", "Agua");
        _sp.Add("Beer", "Cerveza");
        _sp.Add("Honey", "Miel");
        _sp.Add("Bucket", "Cubo");
        _sp.Add("Cart", "Carreta");
        _sp.Add("RoofTile", "Teja");
        _sp.Add("FloorTile", "Azulejo");
        _sp.Add("Mortar", "Mezcla");
        _sp.Add("Furniture", "Muebles");

        _sp.Add("Utensil", "Utensilios");
        _sp.Add("Stop", "Pare");


        //more Main GUI
        _sp.Add("Workers distribution", "Distribución de los trabajadores");
        _sp.Add("Buildings", "Construcciones");

        _sp.Add("Age", "Edad");
        _sp.Add("Gender", "Género");
        _sp.Add("Height", "Altura");
        _sp.Add("Weight", "Peso");
        _sp.Add("Calories", "Calorías");
        _sp.Add("Nutrition", "Nutrición");
        _sp.Add("Profession", "Profesión");
        _sp.Add("Spouse", "Cónyuge");
        _sp.Add("Happinness", "Felicidad");
        _sp.Add("Years Of School", "Años de escuela");
        _sp.Add("Age majority reach", "Mayor de edad");
        _sp.Add("Home", "Hogar");
        _sp.Add("Work", "Trabajo");
        _sp.Add("Food Source", "Almacén");
        _sp.Add("Religion", "Religión");
        _sp.Add("Chill", "Relajamiento");
        _sp.Add("Thirst", "Sed");
        _sp.Add("Account", "Cuenta");

        _sp.Add("Early Access Build", "Acceso Anticipado");

        //Screens
        //Main Menu
        _sp.Add("Resume Game", "Resumir Juego");
        _sp.Add("Continue Game", "Continuar Juego");
        _sp.Add("Tutorial", "Tutorial");
        _sp.Add("New Game", "Juego Nuevo");
        _sp.Add("Load Game", "Cargar Juego");
        _sp.Add("Save Game", "Salvar Juego");
        _sp.Add("Achievements", "Logros");
        _sp.Add("Options", "Opciones");
        _sp.Add("Exit", "Salir");
        //New Game
        _sp.Add("Town Name:", "Pueblo:");
        _sp.Add("Difficulty:", "Dificultad:");
        _sp.Add("Easy", "Fácil");
        _sp.Add("Moderate", "Medio");
        _sp.Add("Hard", "Difícil");
        _sp.Add("Type of game:", "Tipo de juego:");
        _sp.Add("Freewill", "Libre");
        _sp.Add("Traditional", "Tradicional");
        _sp.Add("New.Game.Pirates", "Piratas (si es marcado, pudieses sufrir el ataque de los piratas)");
        _sp.Add("New.Game.Expires", "Caducidad (si lo marcas, la comida tiene fecha de expiración)");
        _sp.Add("OK", "OK");
        _sp.Add("Cancel", "Cancelar");
        _sp.Add("Delete", "Borrar");
        _sp.Add("Enter name...", "Escribe el nombre...");
        //Options
        _sp.Add("General", "General");
        _sp.Add("Unit System:", "Sistema de unidades:");
        _sp.Add("Metric", "Métrico");
        _sp.Add("Imperial", "Imperial");
        _sp.Add("AutoSave Frec:", "Auto salva:");
        _sp.Add("20 min", "20 min");
        _sp.Add("15 min", "15 min");
        _sp.Add("10 min", "10 min");
        _sp.Add("5 min", "5 min");
        _sp.Add("Language:", "Lenguaje:");
        _sp.Add("English", "English");
        _sp.Add("Camera Sensitivity:", "Sensibilidad de la Cámara:");
        _sp.Add("Themes", "Temas");
        _sp.Add("Halloween:", "Halloween:");
        _sp.Add("Christmas:", "Noche Buena:");
        _sp.Add("Options.Change.Theme", "Una vez cambiado por favor reinicia el juego");

        _sp.Add("Screen", "Pantalla");
        _sp.Add("Quality:", "Calidad:");
        _sp.Add("Beautiful", "Magnifico");
        _sp.Add("Fantastic", "Fantástico");
        _sp.Add("Simple", "Simple");
        _sp.Add("Good", "Buena");
        _sp.Add("Resolution:", "Resolución:");
        _sp.Add("FullScreen:", "Pantalla completa:");

        _sp.Add("Audio", "Audio");
        _sp.Add("Music:", "Música:");
        _sp.Add("Sound:", "Sonido:");
        _sp.Add("Newborn", "Bebé");
        _sp.Add("Build Completed", "Construction terminada");
        _sp.Add("People's Voice", "Voz de las personas");
        _sp.Add("Credits", "Créditos");

        //Main Menu Legacy
        _sp.Add("Terrain Name:", "Nombre del terreno:");
        _sp.Add("Click Here", "Haz click aquí");
        _sp.Add("Save Name:", "Nombre de la Salva:");
		//{ "Loading...", "Cargando...");
        _sp.Add("Menu", "Menu");
		//



        //After Oct 20th 2018
        _sp.Add("Resources", "Materiales");
        _sp.Add("Dollars", "Pesetas");
        _sp.Add("Coming.Soon", "Esta construcción ya viene para el juego");
        _sp.Add("Max.Population", "No se pueden construir mas casas. Alcanzaste el límite de personas");

        _sp.Add("To.Unlock", "Desbloquea: ");
        _sp.Add("People", "Personas");
        _sp.Add("Of.Food", " de comida. ");
        _sp.Add("Port.Reputation.Least", "Tu Reputación Portuaria al menos a ");
        _sp.Add("Pirate.Threat.Less", "Amenaza Pirata a menos que ");
        _sp.Add("Skip", "Saltar");

        //After Dec 8, 2018
        _sp.Add("ReloadMod.HoverSmall", "Recarga archivos Mod");
        _sp.Add("isAboveHeight.MaritimeBound", "La sección seca de la construcción esta debajo de la altura mínima");
        _sp.Add("arePointsEven.MaritimeBound", "La sección seca de la construcción no esta en un terreno llano");
        _sp.Add("isOnTheFloor.MaritimeBound", "La sección seca de la construcción no esta en la altura común");
        _sp.Add("isBelowHeight.MaritimeBound", "La sección seca de la construcción no esta en el agua");

        _sp.Add("InLand.Helper", "En Tierra");
        _sp.Add("InWater.Helper", "En Agua");

        //After Dec 28, 2018
        _sp.Add("Down.HoverSmall", "Prioridad Disminuida");
        _sp.Add("Up.HoverSmall", "Prioridad Aumentada");
        _sp.Add("Trash.HoverSmall", "Borrar Orden");
        _sp.Add("Counting...", "Contando...");
        _sp.Add("Ten Orders Limit", "Diez ordenes es el límite");

        //After May 1, 2019
        _sp.Add("Our inventories:", "Nuestros inventarios:");
        _sp.Add("Select Product:", "Selecciona un producto:");
        _sp.Add("Current_Rank.HoverSmall", "Numero en la cola");

        _sp.Add("Construction.Progress", "Progreso de la construcción en: ");
        _sp.Add("Warning.This.Building", "Atención: Esta construcción no puede ser construida ahora. Falta material(es):\n");
        _sp.Add("Product.Selected", "Producto seleccionado: ");
        _sp.Add("Harvest.Date", "\nDia de la recogida: ");
        _sp.Add("Progress", "\nProgreso: ");

        //AddOrderWindow.cs
        _sp.Add("Add.New", "Añade Nueva ");
        _sp.Add("Order", " Orden");
        _sp.Add("Import", "Importa");
        _sp.Add("Export", "Exporta");
        //AddOrderWindow GUI
        _sp.Add("Enter Amount:", "Entra cantidad:");
        _sp.Add("Enter amount...", "Entra cantidad...");
        _sp.Add("New Order:", "Orden Nueva:");
        _sp.Add("Product:", "Producto:");
        _sp.Add("Amount:", "Cantidad:");
        _sp.Add("Order total price:", "Precio total de la orden:");
        _sp.Add("Add", "Add");
        
        //BuildingWindow GUI
        _sp.Add("Product Description:", "Descripción del producto:");
        _sp.Add("Production report by years:", "Reporte anual productivo:");
        _sp.Add("Import Orders", "Importación");
        _sp.Add("Export Orders", "Exportación");
        _sp.Add("Orders in progress:", "Ordenes actuales:");

        //Jun 12, 2019
        _sp.Add("ShowQuest.HoverSmall", "Tarea actual");
        _sp.Add("Have Fun", "Diviértete");
        _sp.Add("Current Quest:", "Tarea actual:");
        _sp.Add("Reward: ", "Premio: ");

        //Nov 5, 2019
        _sp.Add("More", "Mas");
        _sp.Add("Less", "Menos");
        _sp.Add("Reward:", "Premio:");

        //Dec 5, 2019
        _sp.Add("Town", "Pueblo");

        //Dec 9, 2019
        _sp.Add("Barrel", "Barril");
        _sp.Add("Crockery", "Cerámica");
        _sp.Add("WhaleOil", "Aceite");
        _sp.Add("Years of school", "Escolaridad");
        _sp.Add("House comfort", "Calidad de Vivienda");
        _sp.Add("Food source", "Bodega");
        _sp.Add("Relax", "Relajamiento");
        _sp.Add(" of ", " de ");
        _sp.Add("Male", "Masculino");
        _sp.Add("Female", "Femenino");
        _sp.Add("Quenched", "Saciada");



        //Dec 14

        //in game gui

        _sp.Add("Help", "Ayuda");
        _sp.Add("Quest", "Desafió");
        _sp.Add("Add Order", "Pedidos");
        _sp.Add("Suggest Change", "Sugerencias");

        _sp.Add("Panel Control / Bulletin", "Panel de Control");
        _sp.Add("Finance", "Finanzas");
        _sp.Add("Finanzas", "Finanzas");
        _sp.Add("Exports", "Exporta");
        _sp.Add("Ledger", "Cuentas");

        _sp.Add("Prod", "Producto");
        _sp.Add("Consume", "Consumido");
        _sp.Add("Produce", "Producido");
        _sp.Add("Expire", "Caducado");

        _sp.Add("Spec", "Específico");
        _sp.Add("Input1", "Insumo1");
        _sp.Add("Input2", "Insumo2");
        _sp.Add("Input3", "Insumo3");
        _sp.Add("Building", "Edificio");
        _sp.Add("Price", "Precio");

        _sp.Add("Date", "Fecha");
        _sp.Add("Product", "Producto");
        _sp.Add("Amount", "Cantidad");
        _sp.Add("Transaction", "Transacción");

        _sp.Add("Workers", "Trabajos");

        //Help
        _sp.Add("Bulletin", "Boletín");
        _sp.Add("Construction", "Construcción");
        _sp.Add("Emigrate", "Emigracion");
        _sp.Add("Happiness", "Felicidad");
        _sp.Add("Horse Carriages", "Coches de Caballos");
        _sp.Add("Inputs", "Entradas");
        _sp.Add("Line production", "Linea de producción");
        _sp.Add("Our Inventories", "Inventorios");
        _sp.Add("Inventories Explanation", "Inventorios Información");
        _sp.Add("People Range", "Rango Personal");
        _sp.Add("Pirate Threat", "Amenaza Pirata");
        _sp.Add("Population", "Poblacion");
        _sp.Add("Port Reputation", "Reputación Portuaria");
        _sp.Add("Production Tab", "Producción Pestaña");
        _sp.Add("Products Expiration", "Caducidad de productos");
        _sp.Add("Sea Path", "Camino al Mar");
        _sp.Add("Trading", "Comercio");
        _sp.Add("Usage of goods", "Uso de bienes");
        _sp.Add("What is Ft3 and M3?", "Que es Ft3 y M3?");
        _sp.Add("WheelBarrows", "Carretilleros");

        //help
        _sp.Add("Construction.Help", "Para la construcción de cualquier edificio necesita tener trabajadores en la Casa de Albañiles."+
                    "Haga clic en la Casa de Albañiles, luego en el signo '+' en la pestaña general. Asegúrese de tener suficientes recursos");
        _sp.Add("Camera.Help", "Cámara: use [WASD] o el cursor para moverse. "+
                        "Presione la rueda de desplazamiento del mouse, manténgala presionada para girar, o [Q] y [E]");
        _sp.Add("Sea Path.Help", "Haga clic en la esquina inferior izquierda 'Mostrar / ocultar ruta marítima' "+
                            "para mostrar el camino más cercano al mar");

        _sp.Add("People Range.Help", "El enorme círculo azul alrededor de cada edificio marca el alcance del mismo.");

        _sp.Add("Pirate Threat.Help", "Amenaza pirata: conocimiento pirata de tu puerto. Esto aumenta cuando "+
                                        "tienes más dinero. Si esto llega a más de 90, perderás el juego. Puedes contrarrestar la amenaza construyendo edificios militares");

        _sp.Add("Port Reputation.Help", "Reputación de puertos: mientras más personas conozcan su puerto, más visitarán. " +
                                            "Si desea aumentar esto, asegúrese de tener siempre algunos pedidos" +
                                            " en el muelle");
        _sp.Add("Emigrate.Help", "Emigrates: When people are unhappy for a few years they leave. The bad" +
                                    " part of this is they won't come back, they won't produce or have children." +
                                    " The only good thing is that they increase the 'Port Reputation'");
        _sp.Add("Food.Help", "Alimentos: cuanto mayor sea la variedad de alimentos disponibles en un hogar, más felices serán todos");

        _sp.Add("Weight.Help", "Peso: todos los pesos en el juego están en Kg o Lbs, dependiendo del sistema de Unidad seleccionado. " +
                                "Puede cambiarlo en 'Opciones' en el 'Menú principal'");
        _sp.Add("What is Ft3 and M3?.Help", "La capacidad de almacenamiento está determinada por el volumen del edificio. Ft3 es un pie cúbico. M3 es un metro cúbico");//. Keep in mind that less dense products will fill up your storage quickly. To see products density Bulletin/Prod/Spec" );

        _sp.Add("More.Help", "Si necesita más ayuda, puede ser una buena idea completar el tutorial o simplemente publicar una pregunta en los foros de SugarMill");

        //more 
        _sp.Add("Products Expiration.Help", "Vencimiento de productos: al igual que en la vida real, en este juego todos los productos caducan. Algunos alimentos caducan antes que otros. Puede ver cuántos productos han caducado en Boletín / Producto / Caducar");
        _sp.Add("Horse Carriages.Help", "Como el juego tiene medidas reales, las personas solo pueden llevar una cantidad considerable. Entonces es cuando entran en juego los carruajes tirados por caballos. Llevan mucho más, como resultado, su economía se ve impulsada. Una persona en sus mejores años puede transportar alrededor de 15 kg, con una carretilla unos 60 kg, pero el carrito más pequeño de caballos puede transportar 240 kg. Para usarlos, construya 'Cocheros'");
        _sp.Add("Usage of goods.Help", "Uso de bienes: cajas, barriles, carretillas, carros, herramientas, telas, vajillas, muebles y utensilios son necesarios para realizar las actividades tradicionales de una ciudad. A medida que estos productos se utilizan, disminuyen, como resultado, una persona no llevará nada si no hay cajas. Vigila eso;)");
        _sp.Add("Happiness.Help", "Felicidad: la felicidad de las personas está influenciada por varios factores. La cantidad de dinero que tienen, la variedad de alimentos, la satisfacción de la religión, el acceso al ocio, la comodidad de la casa y el nivel educativo. También si una persona tiene acceso a utensilios, vajilla y ropa influirá en su felicidad.");
        _sp.Add("Line production.Help", "Producción en línea: para hacer un clavo simple, necesita extraer mineral, en la fundición derretir el hierro, y finalmente en el herrero hacer el clavo. Si tienes suficiente dinero, siempre puedes comprar el clavo directamente en un barco o cualquier otro producto.");
        _sp.Add("Bulletin.Help", "El icono de páginas en la barra inferior es la Ventana de Boletín. Por favor, tómate un minuto para explorarlo.");
        _sp.Add("Trading.Help", "Necesitarás tener al menos un Puerto para poder comerciar. En él, puede agregar pedidos de importación / exportación y ganar algo de dinero. Si necesita ayuda para agregar un pedido, puede completar el Tutorial");

        _sp.Add("Combat Mode.Help", "It activates when a Pirate/Bandit is detected by one of your citizens. Once the mode is active you can command units directly to attack. Select them and right click to objective to attack");

        _sp.Add("Population.Help", "Una vez que cumplan 16 se mudarán a una casa si la encuentran. Si siempre hay una casa vacia para mudarse, se garantizará el crecimiento de la población. Si se mudan a las nuevas casas a los 16 años, estás maximizando el crecimiento de la población.");


        _sp.Add("F1.Help", "[F1] Ayuda");

        _sp.Add("Inputs.Help", "Si un edificio no se puede construir porque faltan insumos. Verifique que tenga los insumos necesarios en el almacenamiento principal y al menos un trabajador en la Casa de Albañiles");
        _sp.Add("WheelBarrows.Help", "Las carretillas son los trabajadores de albañilería. Si no tienen nada que construir, actuarán como carretilleros. Si necesita insumos para llevar a un edificio específico, asegúrese de tener suficientes de ellas funcionando y también las insumos mencionadas en el edificio de almacenamiento");

        _sp.Add("Production Tab.Help", "Si el edificio es un campo agrícola, asegúrese de tener trabajadores en la granja. La cosecha se perderá si se sienta allí un mes después del día de la recogida.");
        _sp.Add("Our Inventories.Help", "La sección 'Nuestros inventarios' en la 'Ventana Agregar pedido' es un resumen de lo que obtuvimos en nuestros inventarios de edificios de almacenamiento");
        _sp.Add("Inventories Explanation.Help", "Este es un resumen de lo que obtuvimos en nuestros inventarios de almacenamiento. Los artículos en los inventarios de otros edificios no pertenecen a la ciudad.");


        //Dec 16 
        _sp.Add("Town.HoverSmall", "Pueblo");


        //Already in En, Fr, De
        //Bulleting helps
        _sp.Add("Help.Bulletin/Prod/Produce", "Aquí se muestra lo que se produce en el pueblo.");
        _sp.Add("Help.Bulletin/Prod/Expire", "Aquí se muestra lo que ha expirado en el pueblo.");
        _sp.Add("Help.Bulletin/Prod/Consume", "Aquí se muestra lo que está consumiendo su gente.");

        _sp.Add("Help.Bulletin/Prod/Spec", "En esta ventana, puede ver las entradas necesarias para cada producto, dónde se construye y el precio."
            + "Desplácese hacia arriba para ver los encabezados. Observe que un producto puede tener más que una fórmula para producir.");

        _sp.Add("Help.Bulletin/General/Buildings", "Este es un resumen de cuántos edificios hay de cada tipo.");

        _sp.Add("Help.Bulletin/General/Workers", "En esta ventana, puede asignar trabajadores para trabajar en varios edificios."
    + "Para que un edificio permita que más personas trabajen, debe ser inferior a la capacidad y debe encontrar al menos una persona desempleada.");

        _sp.Add("Help.Bulletin/Finance/Ledger", "Aquí se muestra su libro mayor. El salario es la cantidad de dinero pagada a un trabajador. Cuanta más gente trabaje, más salario se pagará.");
        _sp.Add("Help.Bulletin/Finance/Exports", "Detalles de sus exportaciones");
        _sp.Add("Help.Bulletin/Finance/Imports", "Detalles de sus importaciones");

        _sp.Add("Help.Bulletin/Finance/Prices", "....");

        _sp.Add("Decoration.HoverSmall", "Decoraciones");

        _sp.Add("Sand", "Arena");
        _sp.Add("Machinery", "Maquinaria");
        _sp.Add("GunPowder", "Polvora");
        _sp.Add("Cassava", "Yuca");
        _sp.Add("Candy", "Caramelo");

        //All Lang Needed for sure
        
        _sp.Add("Unemployed", "Desempleados");

        //Budget
        _sp.Add("Budget Resumen", "Cuentas");
        _sp.Add("Initial Balance", "Balance Inicial");
        _sp.Add("Income", "Ingresos");
        _sp.Add("Quests Completion", "Desafíos Terminados");
        _sp.Add("Income Subtotal", "Subtotal de Ingresos");

        _sp.Add("Expenses", "Gastos");
        _sp.Add("New bought lands", "Nuevas Tierras Compradas");
        _sp.Add("Salary", "Salarios");
        _sp.Add("Expenses Subtotal", "Subtotal de Gastos");

        _sp.Add("Year", "Año");
        _sp.Add("Imports", "Año");
        _sp.Add("Balance", "Balance");
        _sp.Add("Year Balance", "Balance Anual");
        _sp.Add("Ending Balance", "Balance Final");

        _sp.Add("Construction.HoverMed", "Para construir un edificio necesitas: trabajadores en la Casa de Albañiles y los recursos necesarios");

        _sp.Add("Quest.Arrow", "Desafíos");
        _sp.Add("Prev.HoverSmall", "Anterior");

    }

    internal static void Clear()
    {
        _sp.Clear();
    }

    public static string ReturnValueWithKey(string key)
    {
        return _sp.ReturnValueWithKey(key);
    }

    public static bool ContainsKey(string key)
    {
        return _sp.ContainsKey(key);
    }

}
