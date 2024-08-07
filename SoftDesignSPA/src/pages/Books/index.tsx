// src/pages/BookList.tsx
import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { BooksApi } from '../../apis/books-api';
import { useAuth } from '../../auth/AuthContext';
import {jwtDecode} from 'jwt-decode';

export interface Book {
  id: number;
  name: string;
  author: string;
  tenant: { id: string, email: string } | null;
}

const booksApi = new BooksApi();

const Books: React.FC = () => {
  const { token } = useAuth();
  const [ bookList, setBookList ] = useState<Book[]>([]);
  const [ filteredBooks, setFilteredBooks ] = useState<Book[]>([]);
  const [searchTerm, setSearchTerm] = useState('');
  const decodedToken = jwtDecode(token!) as { sub: string, email: string };
  const userId = decodedToken.sub;
  const email = decodedToken.email;

  const onRentClick = async (bookId: number) => {
    await booksApi.rent(bookId);
    const idx = bookList.findIndex(b => b.id === bookId);
    setBookList(bookList.map((b, bookIdx) => {
      if (bookIdx === idx) {
        b.tenant = {
          email,
          id: userId
        };
      }
      return b;
    }));
  };
  
  const onGiveBackClick = async (bookId: number) => {
    await booksApi.giveBack(bookId);
    const idx = bookList.findIndex(b => b.id === bookId);
    setBookList(bookList.map((b, bookIdx) => {
      if (bookIdx === idx) {
        b.tenant = null;
      }
      return b;
    }));
  };

  useEffect(() => {
    booksApi.getAll().then(res => setBookList(res.data));
  }, []);

  useEffect(() => {
    setFilteredBooks(bookList.filter((book) => book.name.toLowerCase().includes(searchTerm.toLowerCase())));
  }, [bookList, searchTerm]);

  return (
    <div style={{ padding: '20px' }}>
      <h2>Book List</h2>
      <input
        type="text"
        placeholder="Search books"
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
        style={{ marginBottom: '20px', padding: '10px', width: '100%' }}
      />
      <table style={{ width: '100%', borderCollapse: 'collapse' }}>
        <thead>
          <tr>
            <th style={{ border: '1px solid #ddd', padding: '8px' }}>Nome</th>
            <th style={{ border: '1px solid #ddd', padding: '8px' }}>Autor</th>
            <th style={{ border: '1px solid #ddd', padding: '8px' }}>Alugado</th>
            <th style={{ border: '1px solid #ddd', padding: '8px' }}>Ações</th>
          </tr>
        </thead>
        <tbody>
          {filteredBooks && filteredBooks.map((book) => (
            <tr key={book.id}>
              <td style={{ border: '1px solid #ddd', padding: '8px' }}>{book.name}</td>
              <td style={{ border: '1px solid #ddd', padding: '8px' }}>{book.author}</td>
              <td style={{ border: '1px solid #ddd', padding: '8px' }}>{book.tenant ? 'Sim' : 'Não'}</td>
              <td style={{ border: '1px solid #ddd', padding: '8px' }}>
                <Link to={`/books/${book.id}`}>Detalhes</Link>
                {
                  !book.tenant &&
                    <button
                      style={{ marginLeft: '10px' }}
                      onClick={() => onRentClick(book.id)}
                    >
                      Alugar
                    </button>
                }
                {
                  book.tenant && book.tenant.id === userId &&
                    <button
                      style={{ marginLeft: '10px', backgroundColor: 'blue', color: 'white' }}
                      onClick={() => onGiveBackClick(book.id)}
                    >
                      Devolver
                    </button>
                }
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default Books;
