﻿namespace TrackPay.DataStructures
{
    public class Lista<T>
    {
        public Node<T>? cabeza;
        public int cont;

        public int Count => cont;


        public Lista()
        {
            cabeza = null;
            cont = 0;
        }

        public void agregar(T data)
        {
            var nodoNuevo = new Node<T>(data);
            if (cabeza == null)
            {
                cabeza = nodoNuevo; 
                cont++;
                return;
            }

            Node<T> nodoTemp = cabeza;
            
            while (nodoTemp.Sig != null)
            {
                nodoTemp = nodoTemp.Sig; 
            }
            nodoTemp.Sig = nodoNuevo;            
            cont++;
        }

        public void InsertarAlInicio(T data)
        {
            var nuevoNodo = new Node<T>(data);
            nuevoNodo.Sig = cabeza;
            cabeza = nuevoNodo;
            cont++;
        }

        public void InsertarDespues(Node<T> nodoPrevio, T data)
        {
            if (nodoPrevio == null)
                throw new ArgumentNullException(nameof(nodoPrevio));

            var nuevoNodo = new Node<T>(data);
            nuevoNodo.Sig = nodoPrevio.Sig;
            nodoPrevio.Sig = nuevoNodo;
            cont++;
        }

        public T BorrarAlInicio()
        {
            if (cabeza == null)
                throw new InvalidOperationException("La lista está vacía.");

            T datoEliminado = cabeza.Dato; // Guarda el dato del nodo actual
            cabeza = cabeza.Sig;          // Mueve la cabeza al siguiente nodo
            cont--;                       // Reduce el contador
            return datoEliminado;         // Retorna el dato eliminado
        }

        public T BorrarEnIndice(int indice)
        {
            if (indice < 0 || indice >= cont)
                throw new ArgumentOutOfRangeException(nameof(indice));

            if (indice == 0)
            {
                return BorrarAlInicio(); // Usa BorrarAlInicio para el índice 0
            }

            Node<T>? nodoTemp = cabeza;
            for (int i = 0; i < indice - 1; i++)
            {
                nodoTemp = nodoTemp?.Sig;
            }

            // Nodo a eliminar
            var nodoAEliminar = nodoTemp.Sig;
            T datoEliminado = nodoAEliminar.Dato; // Guarda el dato
            nodoTemp.Sig = nodoAEliminar?.Sig;   // Reasigna el enlace
            cont--;                              // Reduce el contador
            return datoEliminado;                // Retorna el dato eliminado
        }

        public T BorrarAlFinal()
        {
            if (cabeza == null)
                throw new InvalidOperationException("La lista está vacía.");

            if (cabeza.Sig == null) // Si solo hay un elemento
            {
                T datoEliminado = cabeza.Dato; // Guarda el dato
                cabeza = null;                 // La lista queda vacía
                cont--;
                return datoEliminado;          // Retorna el dato eliminado
            }

            Node<T>? nodoTemp = cabeza;
            while (nodoTemp?.Sig?.Sig != null) // Encuentra el penúltimo nodo
            {
                nodoTemp = nodoTemp?.Sig;
            }

            T datoEliminadoFinal = nodoTemp.Sig.Dato; // Guarda el dato del último nodo
            nodoTemp.Sig = null;                     // Elimina el último nodo
            cont--;
            return datoEliminadoFinal;               // Retorna el dato eliminado
        }

        public T Borrar(T data)
        {
            if (cabeza == null)
                throw new InvalidOperationException("La lista está vacía.");

            // Si el elemento a borrar está al inicio
            if (cabeza.Dato.Equals(data))
            {
                T datoEliminado = cabeza.Dato;
                cabeza = cabeza.Sig;
                cont--;
                return datoEliminado;
            }

            Node<T>? nodoTemp = cabeza;
            while (nodoTemp?.Sig != null && !nodoTemp.Sig.Dato.Equals(data))
            {
                nodoTemp = nodoTemp.Sig;
            }

            if (nodoTemp?.Sig == null)
                throw new ArgumentException("El dato no existe en la lista.");

            T datoEliminadoFinal = nodoTemp.Sig.Dato;
            nodoTemp.Sig = nodoTemp.Sig.Sig;
            cont--;
            return datoEliminadoFinal;
        }

        public T Get(int indice)
        {
            if (indice < 0 || indice >= cont)
                throw new ArgumentOutOfRangeException(nameof(indice));

            Node<T>? nodeTemp = cabeza;
            for (int i = 0; i < indice; i++)
            {
                nodeTemp = nodeTemp?.Sig;
            }

            return nodeTemp.Dato;
        }

        public bool Existe(T data)
        {
            Node<T>? NodoTemp = cabeza;
            while (NodoTemp != null)
            {
                if (NodoTemp.Dato.Equals(data))
                    return true;

                NodoTemp = NodoTemp.Sig;
            }

            return false;
        }
    }
}
