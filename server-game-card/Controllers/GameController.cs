using Microsoft.AspNetCore.Mvc;

using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

using Newtonsoft.Json;
using System.Diagnostics.Contracts;

using server_game_card.models;

namespace server_game_card.Controllers
{

    [ApiController]
    [Route("Game")]
    public class GameController: ControllerBase
    {
        IFirebaseClient cliente;

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "PU1mvQdxbEEhhdQtIOdNRGGHJg5coQkXTkPdAtvJ",
            BasePath = "https://gamecard-e4146-default-rtdb.firebaseio.com"
        };
        public GameController() {
            cliente = new FirebaseClient(config);
            if (cliente != null)
            {
                Console.WriteLine("Conexión exitosa a Firebase");
            }
            else
            {
                Console.WriteLine("Error al establecer la conexión a Firebase");
            }
        }


        //LISTAR ELEMENTOS
        [HttpPost]
        [Route("listar")]
        public dynamic listarGames()
        {
            try
            {
                //obtener todos los elementos de la base de datos que se encuentran en el conjunto game
                FirebaseResponse response = cliente.Get("game");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Obtener el resultado como un diccionario de Game
                    Dictionary<string, Game> games = response.ResultAs<Dictionary<string, Game>>();

                    // Devolver la lista de games en formato JSON
                    return new
                    {
                        Resultado = games.Select(c => new
                        {
                            IdGame = c.Key,
                            Datos = c.Value
                        }),

                    };
                }
                else
                {
                    // Ocurrió un error al obtener los games
                    return new
                    {
                        Mensaje = "Error al obtener la lista de games",

                    };
                }
            }
            catch (Exception ex)
            {
                // Excepción general
                return new
                {
                    Mensaje = $"Error: {ex.Message}"
                };
            }
        }
        //OBTENER UN ELEMENTO
        [HttpGet]
        [Route("obtener/{id}")]
        public dynamic ObtenerGamePorId(string id)
        {
            try
            {
                // Obtener el game por ID desde la base de datos
                FirebaseResponse response = cliente.Get($"game/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Obtener el resultado como un objeto Contacto
                    Game game = response.ResultAs<Game>();

                    // Devolver el game en formato JSON
                    return new
                    {
                        Resultado = game,
                        Mensaje = $"Game con ID {id} obtenido correctamente"
                    };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // El game con el ID proporcionado no fue encontrado
                    return new
                    {
                        Mensaje = $"No se encontró game con el ID {id}",
                        StatusCode = response.StatusCode
                    };
                }
                else
                {
                    // Ocurrió un error al obtener el game
                    return new
                    {
                        Mensaje = $"Error al obtener game con el ID {id}",
                        StatusCode = response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                // Excepción general
                return new
                {
                    Mensaje = $"Error: {ex.Message}"
                };
            }
        }
        //ELIMINAR ELEMENTO
        [HttpDelete]
        [Route("eliminar/{id}")]
        public dynamic EliminarContactoPorId(string id)
        {
            try
            {
                // Intentar obtener el contacto antes de eliminarlo para verificar si existe
                FirebaseResponse obtenerResponse = cliente.Get($"game/{id}");

                if (obtenerResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // game existe, proceder con la eliminación
                    FirebaseResponse eliminarResponse = cliente.Delete($"game/{id}");

                    if (eliminarResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // Eliminación exitosa
                        return new
                        {
                            Mensaje = $"Contacto con ID {id} eliminado correctamente"
                        };
                    }
                    else
                    {
                        // Ocurrió un error al intentar eliminar el game
                        return new
                        {
                            Mensaje = $"Error al eliminar game con ID {id}",
                           
                        };
                    }
                }
                else if (obtenerResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // game con el ID proporcionado no fue encontrado
                    return new
                    {
                        Mensaje = $"No se encontró game con el ID {id}",
                        StatusCode = obtenerResponse.StatusCode
                    };
                }
                else
                {
                    // Ocurrió un error al intentar obtener el game
                    return new
                    {
                        Mensaje = $"Error al obtener game con el ID {id}",
                        StatusCode = obtenerResponse.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                // Excepción general
                return new
                {
                    Mensaje = $"Error: {ex.Message}"
                };
            }
        }
        //MODIFICAR ELEMENTO
        [HttpPut]
        [Route("modificar/{id}")]
        public dynamic ModificarContactoPorId(string id, [FromBody] Game datosModificados)
        {
            try
            {
                // Intentar obtener game antes de modificarlo para verificar si existe
                FirebaseResponse obtenerResponse = cliente.Get($"game/{id}");

                if (obtenerResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // El game existe, proceder con la modificación
                    FirebaseResponse modificarResponse = cliente.Update($"game/{id}", datosModificados);

                    if (modificarResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // Modificación exitosa
                        return new
                        {
                            Mensaje = $"Game con ID {id} modificado correctamente"
                        };
                    }
                    else
                    {
                        // Ocurrió un error al intentar modificar game
                        return new
                        {
                            Mensaje = $"Error al modificar game con ID {id}",
                            StatusCode = modificarResponse.StatusCode
                        };
                    }
                }
                else if (obtenerResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // game con el ID proporcionado no fue encontrado
                    return new
                    {
                        Mensaje = $"No se encontró game con el ID {id}",
                        StatusCode = obtenerResponse.StatusCode
                    };
                }
                else
                {
                    // Ocurrió un error al intentar obtener game
                    return new
                    {
                        Mensaje = $"Error al obtener game con el ID {id}",
                        StatusCode = obtenerResponse.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                // Excepción general
                return new
                {
                    Mensaje = $"Error: {ex.Message}"
                };
            }
        }

        //GUARDAR ELEMENTO
        [HttpPost]
        [Route("guardar")]
        public dynamic Crear(Game oGame)
        {
            try
            {

                string IdGenerado = Guid.NewGuid().ToString("N");
                SetResponse response = cliente.Set("game/" + IdGenerado, oGame);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Se guardó exitosamente
                    return new
                    {
                        Mensaje = "juego guardado",
                        IdGenerado = IdGenerado
                    };
                }
                else
                {
                    // Ocurrió un error al guardar
                    return new
                    {
                        Mensaje = "Error al guardar game",
                        StatusCode = response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                // Excepción general
                return new
                {
                    Mensaje = $"Error: {ex.Message}"
                };
            }
        }
    }

}
