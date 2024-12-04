using System.Collections;
using System.Collections.Generic;

namespace TrackPay.DataStructures
{
    public class ColaConPrioridad<T> : IEnumerable<T>
    {
        private Lista<ElementoConPrioridad<T>> lista;

        public int Count => lista.Count;

        public ColaConPrioridad()
        {
            lista = new Lista<ElementoConPrioridad<T>>();
        }

        /// <summary>
        /// Implementación del enumerador para iterar sobre los elementos de la cola.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            var nodoActual = lista?.cabeza;

            while (nodoActual != null)
            {
                // Retornar solo el dato, sin exponer la clase interna
                yield return nodoActual.Dato.Dato;
                nodoActual = nodoActual.Sig;
            }
        }

        /// <summary>
        /// Implementación no genérica del enumerador requerida por IEnumerable.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Agrega un elemento a la cola con una prioridad especificada.
        /// </summary>
        public void Enqueue(T dato, DateTime fecha, int bitPrioridad)
        {
            var nuevoElemento = new ElementoConPrioridad<T>(dato, fecha, bitPrioridad);

            if (lista.Count == 0)
            {
                lista.agregar(nuevoElemento);
                return;
            }

            Node<ElementoConPrioridad<T>>? nodoActual = lista?.cabeza;
            Node<ElementoConPrioridad<T>>? nodoPrevio = null;

            // Buscar la posición correcta basada en las prioridades
            while (nodoActual != null)
            {
                if (nodoActual.Dato.Fecha > fecha ||
                   (nodoActual.Dato.Fecha == fecha && nodoActual.Dato.BitPrioridad >= bitPrioridad))
                {
                    nodoPrevio = nodoActual;
                    nodoActual = nodoActual.Sig;
                }
                else
                {
                    break;
                }
            }

            if (nodoPrevio == null)
            {
                lista.InsertarAlInicio(nuevoElemento);
            }
            else
            {
                lista.InsertarDespues(nodoPrevio, nuevoElemento);
            }
        }

        public T Dequeue()
        {
            if (lista.Count == 0)
                throw new InvalidOperationException("La cola está vacía.");

            
            ElementoConPrioridad<T> elemento = lista.BorrarAlInicio();
            T dato = elemento.Dato;
            return dato;                
        }

        public T Peek()
        {
            if (lista.Count == 0)
                throw new InvalidOperationException("La cola está vacía.");

            return lista.Get(0).Dato;
        }

        public bool EsVacia()
        {
            return lista.Count == 0;
        }

        internal class ElementoConPrioridad<T>
        {
            public T Dato { get; set; }
            public DateTime Fecha { get; set; }
            public int BitPrioridad { get; set; }

            public ElementoConPrioridad(T dato, DateTime fecha, int bitPrioridad)
            {
                Dato = dato;
                Fecha = fecha;
                BitPrioridad = bitPrioridad;
            }
        }
    }
}
