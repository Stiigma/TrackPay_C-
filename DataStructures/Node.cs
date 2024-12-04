namespace TrackPay.DataStructures
{
    public class Node<T>
    {
        public T Dato { get; set; }
        public Node<T>? Sig { get; set; }

        public Node(T dato)
        {
            Dato = dato;
            Sig = null;
        }


    }
}
