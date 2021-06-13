# AppFinalPatrones

Esta app enfocada para dispositivos android es una práctica que hice para ver el funcionamiento de la nube Azure, los blobs, el reutilizar interfaces para mostar información,
entre otras características.

Estructura general:
Activity Main - Login
||Main Activity.cs||
	Menu
  -Menu.cs
		*Interfaz de consulta
			-ListaTrabajadores.xml - Trabajador Details.xml 
      -ConsultaTrabajadores.cs
      -DataAdapter.cs
				+ConsultaTrabajador.xml DetallesTrabajadorConsulta.xml
					-DetallesTrabajadorConsulta.cs
		*Registro
      -RegistroTrabajador.cs
    
    *Movimientos
      -ListaMovimientos.xml - Movimientos Details.xml
        -ConsultaMovimientos.cs
        -MovAdapter.cs
       
