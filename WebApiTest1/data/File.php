<?
// Script para generar archivos a SAP en TW, se invoca desde TW y desde POS ??
// 17 junio 2013:
//   se modifico criterio de disponibilidad de prodcutos, ya no es fijo (config), sino de la categoria

// recorrer detalle de pedido, para armar final de TXT
$max_entrega_linea_recoge = 0;
$max_entrega_linea_domicilio = 0;
$max_entrega_desc_recoge = 0;
$max_entrega_desc_domicilio = 0;

$ren_linea_domicilio = '';
$ren_linea_recoge = '';
$ren_descontinuados_domicilio = '';
$ren_descontinuados_recoge= '';
$hay_linea_recoge = 0;
$hay_linea_domicilio = 0;
$hay_descontinuados_domicilio = 0;
$hay_descontinuados_recoge = 0;
$ren_garantias = '';
$hay_garantias = 0;
$textos_garantias = '';
$hay_fletes = 0;
$ren_fletes = '';

//$CR = "\r\n";
$IL = "^";      // inicio de línea
$CR = "^\n";    // separador especifico que solicitan

$total_domicilio_resurtible = 0;
$total_domicilio_no_resurtible = 0;
$total_garantias = 0;
$total_fletes = 0;
$sucursal_ocurre = 0;

$resultadoDP = mysql_query("SELECT detalle_pedido.*, producto.resurtible, producto.categoria
							  FROM detalle_pedido 
							  LEFT JOIN producto ON detalle_pedido.modelo = producto.modelo
							 WHERE pedido = $folio_pedido ORDER BY partida",$conexion);

while ($rowDP = mysql_fetch_array($resultadoDP)) {
	$modelo = $rowDP['modelo'];
	$entrega = $rowDP['entrega'];
	$cedis = $rowDP['cedis'];
	
	// los loc = 0 (de productos como garantías o que no hay existencia pero es resurtible, ponerlo en 1 para que se vaya a SAP con 1)
	$loc = str_pad($rowDP['loc'],4,'0',STR_PAD_LEFT);
	if ($loc == '0000') $loc_x = '0001'; else $loc_x = $loc;
	$fecha_pedido_ = explode("-",substr($row['fecha'], 0,10));
	$fecha_entrega = date ( "Y-m-d", mktime(0,0,0,$fecha_pedido_[1],$fecha_pedido_[2]+$entrega,$fecha_pedido_[0] ));

	$categoria = $rowDP['categoria'];
	$resultadoCAT = mysql_query("SELECT minimo, tipo_inventario FROM categoria WHERE clave = $categoria");
	$rowCAT = mysql_fetch_array($resultadoCAT);
	$disponibilidad_venta = $rowCAT['minimo'];
	$tipo_inventario = $rowCAT['tipo_inventario'];


	if ($rowDP['combo']>0) {
		// tomar lista de precio del combo, no de la empresa
		$lista_precios_x = strtoupper(trim($rowDP['lista_precios']));
		if ($lista_precios_x == 'WEB') $lista_precios_x = 'TE';
		// echo "<br>".$modelo." combo: ".$rowDP['combo']." lista: ".$rowDP['lista_precios']." - ".$lista_precios_x;
		
	} else {
		// tomarla del pedido, por si se cambió por precio especial
		$lista_precios_x = strtoupper(trim($rowDP['lista_precios']));
		if ($lista_precios_x == 'WEB') $lista_precios_x = 'TE';
		if (!$lista_precios_x) $lista_precios_x = $lista_precios;
	}
		
	if ($rowDP['es_garantia']) { 
			$max_entrega_linea = $entrega;
			$hay_garantias++;
			$ren_garantias .= $IL.$rowDP['modelo'].$CR;		// MATERIAL   	--> modelo
			$ren_garantias .= $IL.'RM11'.$CR;				// PLANT  		--> cedis
			$ren_garantias .= $IL.$CR;						// STORE_LOC	--> store location  en garantía va en blanco
			$ren_garantias .= $IL.$rowDP['cantidad'].$CR;	// TARGET_QTY   --> cantidad (1)
			$ren_garantias .= $IL.$lista_precios_x.$CR;		// Lista Precios
			$ren_garantias .= $IL.$CR;	// ENTREGA EXPRES   --> cantidad (1)
			$ren_garantias .= $IL.$CR;	// TURNO DE ENTREGA   --> cantidad (1)
			if ($textos_garantias) $textos_garantias.='|';  // separar con pipe cada garantía. Si ya traía otra garantía, agregamos primero el separador
			$textos_garantias .= $rowDP['folio_garantia']." - ".$rowDP['rel_garantia']." - $".$rowDP['precio_empleado'];

			$total_garantias += $rowDP['precio_empleado']; 
	
	} else {
	
	
		// detectar tipo de producto
		if ($rowDP['tipo_entrega'] == 'domicilio') {
		
			// detectar máximo tiempo de entrega
			$es_resurtible = $rowDP['resurtible'];
			if ($es_resurtible) {
				if ($entrega > $max_entrega_linea_domicilio) $max_entrega_linea_domicilio = $entrega;
				$hay_linea_domicilio ++;
				$ren_linea_domicilio .= $IL.$rowDP['modelo'].$CR;		// MATERIAL   	--> modelo
				$ren_linea_domicilio .= $IL.$cedis.$CR;				// PLANT  		--> cedis
				$ren_linea_domicilio .= $IL.$loc_x.$CR;				// STORE_LOC	--> store location
				$ren_linea_domicilio .= $IL.$rowDP['cantidad'].$CR;	// TARGET_QTY   --> cantidad (1)
				$ren_linea_domicilio .= $IL.$lista_precios_x.$CR;		// Lista Precios
				$ren_linea_domicilio .= $IL.$CR;	// ENTREGA EXPRES   --> cantidad (1)
				$ren_linea_domicilio .= $IL.$CR;	// TURNO DE ENTREGA   --> cantidad (1)
				$total_domicilio_resurtible += $rowDP['precio_empleado']; 

			} else {
				if ($entrega > $max_entrega_desc_domicilio) $max_entrega_desc_domicilio = $entrega;
				$hay_descontinuados_domicilio ++;
				$ren_descontinuados_domicilio .= $IL.$rowDP['modelo'].$CR;	// MATERIAL   	--> modelo
				$ren_descontinuados_domicilio .= $IL.$cedis.$CR;				// PLANT  		--> cedis
				$ren_descontinuados_domicilio .= $IL.$loc_x.$CR;				// STORE_LOC	--> store location
				$ren_descontinuados_domicilio .= $IL.$rowDP['cantidad'].$CR;	// TARGET_QTY   --> cantidad (1)
				$ren_descontinuados_domicilio .= $IL.$lista_precios_x.$CR;		// Lista precios
				$ren_descontinuados_domicilio .= $IL.$CR;	// ENTREGA EXPRES   --> cantidad (1)
				$ren_descontinuados_domicilio .= $IL.$CR;	// TURNO DE ENTREGA   --> cantidad (1)
				$total_domicilio_no_resurtible += $rowDP['precio_empleado']; 
			}
			
		} else { // cliente recoge ocurre
		
			// detectar máximo tiempo de entrega
			$es_resurtible = $rowDP['resurtible'];
			if ($es_resurtible) {
				if ($entrega > $max_entrega_linea_recoge) $max_entrega_linea_recoge = $entrega;
				$hay_linea_recoge ++;
				$ren_linea_recoge .= $IL.$rowDP['modelo'].$CR;		// MATERIAL   	--> modelo
				$ren_linea_recoge .= $IL.$cedis.$CR;				// PLANT  		--> cedis
				$ren_linea_recoge .= $IL.$loc_x.$CR;				// STORE_LOC	--> store location
				$ren_linea_recoge .= $IL.$rowDP['cantidad'].$CR;	// TARGET_QTY   --> cantidad (1)
				$ren_linea_recoge .= $IL.$lista_precios_x.$CR;		// Lista Precios
				$ren_linea_recoge .= $IL.$CR;	// ENTREGA EXPRES   --> cantidad (1)
				$ren_linea_recoge .= $IL.$CR;	// TURNO DE ENTREGA   --> cantidad (1)
				$total_recoge_resurtible += $rowDP['precio_empleado']; 

			} else {
				if ($entrega > $max_entrega_desc_recoge) $max_entrega_desc_recoge = $entrega;
				$hay_descontinuados_recoge ++;
				$ren_descontinuados_recoge .= $IL.$rowDP['modelo'].$CR;	// MATERIAL   	--> modelo
				$ren_descontinuados_recoge .= $IL.$cedis.$CR;				// PLANT  		--> cedis
				$ren_descontinuados_recoge .= $IL.$loc_x.$CR;				// STORE_LOC	--> store location
				$ren_descontinuados_recoge .= $IL.$rowDP['cantidad'].$CR;	// TARGET_QTY   --> cantidad (1)
				$ren_descontinuados_recoge .= $IL.$lista_precios_x.$CR;		// Lista precios
				$ren_descontinuados_recoge .= $IL.$CR;	// ENTREGA EXPRES   --> cantidad (1)
				$ren_descontinuados_recoge .= $IL.$CR;	// TURNO DE ENTREGA   --> cantidad (1)
				$total_recoge_no_resurtible += $rowDP['precio_empleado']; 

			}
			if ($rowDP['tipo_entrega'] == 'ocurre' && $sucursal_ocurre==0 && $rowDP['sucursal_ocurre']>0) {
				$sucursal_ocurre = $rowDP['sucursal_ocurre'];
			}
			
		}

		if ($rowDP['costo_entrega']>0 && $rowDP['sku_entrega']!='') { 
			$hay_fletes ++;
			$ren_fletes.= $IL.$rowDP['sku_entrega'].$CR;		// MATERIAL   	--> modelo
			$ren_fletes.= $IL.'RM11'.$CR;					// PLANT  		--> cedis
			$ren_fletes.= $IL.$CR;							// STORE_LOC	--> store location  en garantía va en blanco
			$ren_fletes.= $IL."1".$CR;						// TARGET_QTY   --> cantidad (1)
			$ren_fletes .= $IL.$lista_precios_x.$CR;			// Lista Precios, misma que la del producto
			$ren_fletes .= $IL.$CR;	// ENTREGA EXPRES   --> cantidad (1)
			$ren_fletes .= $IL.$CR;	// TURNO DE ENTREGA   --> cantidad (1)
			$total_fletes += $rowDP['costo_entrega']; 		// antes precio_empleado

		}

		// disminuir existencias
		$loc_exi = $loc+0; // convertir a integer
		if ($loc_exi>0) {

			$query = "UPDATE existencia SET existencia = existencia -1 
										WHERE producto = '$modelo'
										  AND cedis = '$cedis'
										  AND loc = $loc_exi LIMIT 1";
			$resultadoDE = mysql_query($query,$conexion);
										  
			$upd_ex = mysql_affected_rows();
	
			if ($upd_ex <= 0) {
				// $error .= "<br>No se pudo disminuir existencia de producto";
				
			} else {
				// actualizar si el producto aun debe seguir mostrándose (solo si no es resurtible y ya no hay existencias)
				if (!$es_resurtible || 1 ) {  // Ahora solo importa la existencia para mostrarlo o no


					$producto = $modelo;
					// $categoria = $categoria; // ya se obtuvo arriba
					include("admin/disponibilidad_prod.php"); // requiere $categoria y $producto, devuelve $mostrar y $ocultar

					$resultadoUP = mysql_query("UPDATE producto SET mostrar = $mostrar, ocultar = $ocultar WHERE modelo = '$modelo' LIMIT 1",$conexion);
				
				} // es resurtible
			} // se elimino existencia
		} // if loc > 0
		
		
	}  // es garantia
	

} // while rowDP

// crear datos generales del TXT
//				$fecha_arch = date("ymd");  // no debe ser la fecha de hoy, sino la del pedido
$fecha_arch = str_replace('-','',$row['fecha']);
$fecha_arch = substr($fecha_arch,2,10);
//$delivery_date = date ( "Ymd", mktime(0,0,0,date("m"),date("d")+1,date("Y") ));
	$fecha_pedido_ = explode("-",substr($row['fecha'], 0,10));
	$delivery_date = date ( "Ymd", mktime(0,0,0,$fecha_pedido_[1],$fecha_pedido_[2]+1,$fecha_pedido_[0] ));


// obtener datos de sucursal ocurre en caso que haya
if ($sucursal_ocurre) { 
	$resultadoSA = mysql_query("SELECT * FROM sucursal_ocurre WHERE clave = $sucursal_ocurre");
	$rowSA = mysql_fetch_array($resultadoSA);
}

// obtener datos de cliente  (( cuando está en la tienda, lo trae en sesión.. cuando se ejecuta desde admin (validar pago) se trae en variable..
if (!$clave_cliente)
	$clave_cliente = $_SESSION['cliente_valido'];


if ($hay_linea_domicilio) {
	$nombre_archivo_linea = "TAWOrder".$fecha_arch.$folio_pedido."_LD.txt";
	
	// calcular fecha máxima de envío de línea
		$fecha_pedido_ = explode("-",substr($row['fecha'], 0,10));
	$fecha_maxima_entrega = date ( "Ymd", mktime(0,0,0,$fecha_pedido_[1],$fecha_pedido_[2]+$max_entrega_linea_domicilio,$fecha_pedido_[0] ));
	//$fecha_maxima_entrega = date ( "Ymd", mktime(0,0,0,date("m"),date("d")+$max_entrega_linea_domicilio,date("Y") ));
	$ren = '';
//	$clave_cliente = $_SESSION['cliente_valido'];
	$ship_cond = 'SD'; // Standard Delivery
	$sufijo = '_LD';
	$total_archivo = $total_domicilio_resurtible;
	$folio_archivo = $fecha_arch.$folio_pedido."_LD.txt";

	include($url_tw."/datos_pedido_header_v2.php");

	$contenido = $IL.'ZWIO'.$CR;   // es ZWIO para resurtible y ZWIO para no resurtible, pero puede haber de ambos en el pedido
	$contenido .= $ren;
	$contenido .= $IL.$CR;		// REQUEST DELIVERY DATE INITIAL   	--> modelo
	$contenido .= $IL.$CR;		// REQUEST DELIVERY DATE FINAL   	--> modelo
	$fecha_d = str_replace('-','',substr($row['fecha'],0,10));
	$contenido .= $IL.$CR;		// PRICING DATE   	--> modelo
	$contenido .= $IL.$CR;		// INCOTERM   Entrega Inmediata - Blank LI y/o DI, Entrega a Domicilio - CIF LD y/o DD, Entrega en Planta - FOB LO y/o DO
	$prod_junt = ($row['productos_juntos']) ? 'X' : '' ;
	$contenido .= $IL.$CR;		// ORDER COMPLETE   	--> modelo
	$contenido .= $ren_linea_domicilio;
	
	//$url_admin = './admin'; se invoca desde admin también con el valor '.'
	$handle = fopen($url_admin."/exp_ped/".$nombre_archivo_linea, "wb"); // or die ("Problemas para generar archivo");
   
	if ($handle) {
	   fputs($handle,$contenido);
	   fclose($handle);
	} else { 
	   $error .= "No se pudo crear el archivo TXT para exportación productos de línea";
	}
	
} // hay linea

if ($hay_descontinuados_domicilio) {
	$nombre_archivo_desc = "TAWOrder".$fecha_arch.$folio_pedido."_DD.txt";
	
	// calcular fecha máxima de envío para descontinuados
			$fecha_pedido_ = explode("-",substr($row['fecha'], 0,10));
	$fecha_maxima_entrega = date ( "Ymd", mktime(0,0,0,$fecha_pedido_[1],$fecha_pedido_[2]+$max_entrega_desc_domicilio,$fecha_pedido_[0] ));
	//$fecha_maxima_entrega = date ( "Ymd", mktime(0,0,0,date("m"),date("d")+$max_entrega_desc_domicilio,date("Y") ));
	$ship_cond = 'SD'; // Standard Delivery
	$ren = '';
	$sufijo = '_DD';
	$total_archivo = $total_domicilio_no_resurtible;
	$folio_archivo = $fecha_arch.$folio_pedido."_DD.txt";

	include($url_tw."/datos_pedido_header_v2.php");

	$contenido = $IL.'ZWIO'.$CR;   // es ZWIO para resurtible y ZWIO para no resurtible, pero puede haber de ambos en el pedido
	$contenido .= $ren;
	$contenido .= $IL.$CR;		// REQUEST DELIVERY DATE INITIAL   	--> modelo
	$contenido .= $IL.$CR;		// REQUEST DELIVERY DATE FINAL   	--> modelo
	$fecha_d = str_replace('-','',substr($row['fecha'],0,10));
	$contenido .= $IL.$CR;		// PRICING DATE   	--> modelo
	$contenido .= $IL.$CR;		// INCOTERM   Entrega Inmediata - Blank LI y/o DI, Entrega a Domicilio - CIF LD y/o DD, Entrega en Planta - FOB LO y/o DO
	$prod_junt = ($row['productos_juntos']) ? 'X' : '' ;
	$contenido .= $IL.$CR;		// ORDER COMPLETE   	--> modelo
	$contenido .= $ren_descontinuados_domicilio;

	$handle = fopen($url_admin."/exp_ped/".$nombre_archivo_desc, "wb"); // or die ("Problemas para generar archivo");
   
	if ($handle) {
	   fputs($handle,$contenido);
	   fclose($handle);
	} else { 
	   $error .= "No se pudo crear el archivo TXT para exportación productos de línea";
	}
	
} // hay descontinuados

if ($hay_linea_recoge) {
	$nombre_archivo_linea = "TAWOrder".$fecha_arch.$folio_pedido."_LR.txt";
	
	// calcular fecha máxima de envío de línea
				$fecha_pedido_ = explode("-",substr($row['fecha'], 0,10));
	$fecha_maxima_entrega = date ( "Ymd", mktime(0,0,0,$fecha_pedido_[1],$fecha_pedido_[2]+$max_entrega_linea_recoge,$fecha_pedido_[0] ));
	//$fecha_maxima_entrega = date ( "Ymd", mktime(0,0,0,date("m"),date("d")+$max_entrega_linea_recoge,date("Y") ));
	$ren = '';
//	$clave_cliente = $_SESSION['cliente_valido'];
	$ship_cond = 'NS'; // do Not Ship
	$sufijo = '_LR';
	$total_archivo = $total_recoge_resurtible;
	$folio_archivo = $fecha_arch.$folio_pedido."_LR.txt";

	include($url_tw."/datos_pedido_header_v2.php");

	$contenido = $IL.'ZWIO'.$CR;   // es ZWIO para resurtible y ZWIO para no resurtible, pero puede haber de ambos en el pedido
	$contenido .= $ren;
	$contenido .= $IL.$CR;		// REQUEST DELIVERY DATE INITIAL   	--> modelo
	$contenido .= $IL.$CR;		// REQUEST DELIVERY DATE FINAL   	--> modelo
	$fecha_d = str_replace('-','',substr($row['fecha'],0,10));
	$contenido .= $IL.$CR;		// PRICING DATE   	--> modelo
	$contenido .= $IL.$CR;		// INCOTERM   Entrega Inmediata - Blank LI y/o DI, Entrega a Domicilio - CIF LD y/o DD, Entrega en Planta - FOB LO y/o DO
	$prod_junt = ($row['productos_juntos']) ? 'X' : '' ;
	$contenido .= $IL.$CR;		// ORDER COMPLETE   	--> modelo
	$contenido .= $ren_linea_recoge;

	$handle = fopen($url_admin."/exp_ped/".$nombre_archivo_linea, "wb"); // or die ("Problemas para generar archivo");
   
	if ($handle) {
	   fputs($handle,$contenido);
	   fclose($handle);
	} else { 
	   $error .= "No se pudo crear el archivo TXT para exportación productos de línea";
	}
	
} // hay linea

if ($hay_descontinuados_recoge) {
	$nombre_archivo_desc = "TAWOrder".$fecha_arch.$folio_pedido."_DR.txt";
	
	// calcular fecha máxima de envío para descontinuados
	$fecha_pedido_ = explode("-",substr($row['fecha'], 0,10));
	$fecha_maxima_entrega = date ( "Ymd", mktime(0,0,0,$fecha_pedido_[1],$fecha_pedido_[2]+$max_entrega_desc_recoge,$fecha_pedido_[0] ));
	//$fecha_maxima_entrega = date ( "Ymd", mktime(0,0,0,date("m"),date("d")+$max_entrega_desc_recoge,date("Y") ));
	$ship_cond = 'NS'; // do Not Ship
	$ren = '';
	$sufijo = '_DR';
	$total_archivo = $total_recoge_no_resurtible;
	$folio_archivo = $fecha_arch.$folio_pedido."_DR.txt";

	include($url_tw."/datos_pedido_header_v2.php");

	$contenido = $IL.'ZWIO'.$CR;   // es ZWIO para resurtible y ZWIO para no resurtible, pero puede haber de ambos en el pedido
	$contenido .= $ren;
	$contenido .= $IL.$CR;		// REQUEST DELIVERY DATE INITIAL   	--> modelo
	$contenido .= $IL.$CR;		// REQUEST DELIVERY DATE FINAL   	--> modelo
	$fecha_d = str_replace('-','',substr($row['fecha'],0,10));
	$contenido .= $IL.$CR;		// PRICING DATE   	--> modelo
	$contenido .= $IL.$CR;		// INCOTERM   Entrega Inmediata - Blank LI y/o DI, Entrega a Domicilio - CIF LD y/o DD, Entrega en Planta - FOB LO y/o DO
	$prod_junt = ($row['productos_juntos']) ? 'X' : '' ;
	$contenido .= $IL.$CR;		// ORDER COMPLETE   	--> modelo
	$contenido .= $ren_descontinuados_recoge;

	$handle = fopen($url_admin."/exp_ped/".$nombre_archivo_desc, "wb"); // or die ("Problemas para generar archivo");
   
	if ($handle) {
	   fputs($handle,$contenido);
	   fclose($handle);
	} else { 
	   $error .= "No se pudo crear el archivo TXT para exportación productos de línea";
	}

} // hay descontinuados				

if ($hay_garantias || $hay_fletes) {
	$nombre_archivo_garantias = "TAWOrder".$fecha_arch.$folio_pedido."_G.txt";
	
	// calcular fecha máxima de envío de línea
	$fecha_pedido_ = explode("-",substr($row['fecha'], 0,10));
	$fecha_maxima_entrega = date ( "Ymd", mktime(0,0,0,$fecha_pedido_[1],$fecha_pedido_[2]+$max_entrega_garantia,$fecha_pedido_[0] ));
	//$fecha_maxima_entrega = date ( "Ymd", mktime(0,0,0,date("m"),date("d")+$max_entrega_garantia,date("Y") ));
	$ship_cond = 'NS'; // do Not Ship
	$ren = '';
//	$clave_cliente = $_SESSION['cliente_valido'];
	$sufijo = '_G';
	$total_archivo = $total_garantias+$total_fletes;
	$folio_archivo = $fecha_arch.$folio_pedido."_G.txt";

	include($url_tw."/datos_pedido_header_v2.php");

	$contenido = $IL.'ZWIO'.$CR;   // es ZWIO para resurtible y ZWIO para no resurtible, pero puede haber de ambos en el pedido
	$contenido .= $ren;
	$contenido .= $IL.$CR;		// REQUEST DELIVERY DATE INITIAL   	--> modelo
	$contenido .= $IL.$CR;		// REQUEST DELIVERY DATE FINAL   	--> modelo
	$fecha_d = str_replace('-','',substr($row['fecha'],0,10));
	$contenido .= $IL.$CR;		// PRICING DATE   	--> modelo
	$contenido .= $IL.$CR;		// INCOTERM   Entrega Inmediata - Blank LI y/o DI, Entrega a Domicilio - CIF LD y/o DD, Entrega en Planta - FOB LO y/o DO
	$prod_junt = ($row['productos_juntos']) ? 'X' : '' ;
	$contenido .= $IL.$CR;		// ORDER COMPLETE   	--> modelo
	$contenido .= $ren_garantias;
	$contenido .= $ren_fletes;

	$handle = fopen($url_admin."/exp_ped/".$nombre_archivo_garantias, "wb"); // or die ("Problemas para generar archivo");
   
	if ($handle) {
	   fputs($handle,$contenido);
	   fclose($handle);
	} else { 
	   $error .= "No se pudo crear el archivo TXT para exportación de garantias / fletes";
	}
	
} // hay garantias	



?>