import React, { useEffect } from 'react';
import { Link, useParams } from 'react-router-dom';
import { BooksApi } from '../../apis/books-api';
import { Book } from '../Books';
const booksApi = new BooksApi();

const BookDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [book, setBook] = React.useState<Book | null>(null);

  useEffect(() => {
    booksApi.getById(Number(id))
      .then((res) => {
        console.log(res);
        setBook(res.data);
      });
  }, [id]);

  return (
    <div style={{ padding: '20px' }}>
      <Link to={`/books`}>Voltar</Link>
      <h2>Book Details</h2>
      {
        book && (
          <>
            <p><strong>Nome:</strong> {book.name}</p>
            <p><strong>Autor:</strong> {book.author}</p>
            <p><strong>Alugado Por:</strong> {book.tenant ? book.tenant.email : 'Não Alugado'}</p>
          </>
        )
      }
    </div>
  );
};

export default BookDetail;
