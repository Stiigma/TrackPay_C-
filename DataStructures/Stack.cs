namespace TrackPay.DataStructures
{
    public class Pila<T>
    {
        public Node<T>? top;
        public int cont;

        public int Count => cont;

        public Pila()
        {
            top = null;
            cont = 0;
        }

        public bool esVacia()
        {
            return top == null;
        }

        public void Push(T dato)
        {
            var nodoNuevo = new Node<T>(dato);
            nodoNuevo.Sig = top;
            top = nodoNuevo;
            cont++;


        }

        public T Pop()
        {
            if (esVacia())
                throw new InvalidOperationException("Stack is empty.");

            T data = top.Dato;
            top = top?.Sig;
            cont--;
            return data;
        }

        public T Peek()
        {
            if (esVacia())
                throw new InvalidOperationException("Stack is empty.");

            return top.Dato;
        }
    }
}
