using System.Collections;

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


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enqueue(T dato, DateTime fecha, int bitPrioridad)
        {

            fecha = fecha.Date;

            var nuevoElemento = new ElementoConPrioridad<T>(dato, fecha, bitPrioridad);

            if (lista.Count == 0)
            {
                lista.agregar(nuevoElemento);
                return;
            }

            Node<ElementoConPrioridad<T>>? nodoActual = lista?.cabeza;
            Node<ElementoConPrioridad<T>>? nodoPrevio = null;


            while (nodoActual != null)
            {

                var fechaActual = nodoActual.Dato.Fecha.Date;

                if (fechaActual < fecha ||
                   (fechaActual == fecha && nodoActual.Dato.BitPrioridad < bitPrioridad))
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
