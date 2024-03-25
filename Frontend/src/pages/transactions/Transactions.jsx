import React, { useState } from 'react';

const InventoryManagementPage = () => {
  const [selectedTransaction, setSelectedTransaction] = useState(null);
  const [newProduct, setNewProduct] = useState({ product: '', quantity: '' });

  // Dummy data - first html&css then I will implement real data
  const transactions = [
    { id: 1, employee: 'John Doe', date: '2024-03-25', status: 'Closed' },
    { id: 2, employee: 'Jane Smith', date: '2024-03-24', status: 'Open' },
  ];

  const transactionItems = {
    1: {
      "Warehouse 1": [
        { id: 1, product: 'Product A', quantity: 10 },
        { id: 2, product: 'Product B', quantity: 20 },
        { id: 3, product: 'Product C', quantity: 15 },
      ],
      "Warehouse 2": [
        { id: 4, product: 'Product D', quantity: 5 },
        { id: 5, product: 'Product E', quantity: 10 },
        { id: 6, product: 'Product F', quantity: 8 },
      ],
    },
    2: {
      "Warehouse 1": [
        { id: 7, product: 'Product G', quantity: 12 },
        { id: 8, product: 'Product H', quantity: 18 },
        { id: 9, product: 'Product I', quantity: 20 },
      ],
      "Warehouse 2": [
        { id: 10, product: 'Product J', quantity: 7 },
        { id: 11, product: 'Product K', quantity: 15 },
        { id: 12, product: 'Product L', quantity: 10 },
      ],
    },
  };

  const handleTransactionClick = (transactionId) => {
    setSelectedTransaction(transactionId);
  };

  const handleCloseTransaction = () => {
    setSelectedTransaction(null);
  };

  return (
    <div className="inventory-management">
      <div className="transaction-list-container">
        <div className="transaction-list">
          <h2>Transactions</h2>
          <ul>
            {transactions.map((transaction) => (
              <li
                key={transaction.id}
                onClick={() => handleTransactionClick(transaction.id)}
                className={selectedTransaction === transaction.id ? 'selected' : ''}
              >
                {`Transaction ID: ${transaction.id}, Employee: ${transaction.employee}, Date: ${transaction.date}, Status: ${transaction.status}`}
              </li>
            ))}
          </ul>
        </div>
      </div>
      <div className="transaction-details-container">
        {selectedTransaction && (
          <div className="transaction-details">
            <h2>Transaction Details</h2>
            <button onClick={handleCloseTransaction}>Close Transaction</button>
            {Object.entries(transactionItems[selectedTransaction]).map(([warehouse, items]) => (
              <div key={warehouse}>
                <h3>{`Warehouse: ${warehouse}`}</h3>
                <ul>
                  {items.map((item) => (
                    <li key={item.id}>
                      {`Product: ${item.product}, Quantity: ${item.quantity}`}
                      <button>Delete</button>
                    </li>
                  ))}
                </ul>
                <input type="text" placeholder="Product Name" value={newProduct.product} onChange={(e) => setNewProduct({ ...newProduct, product: e.target.value })} />
                <input type="number" placeholder="Quantity" value={newProduct.quantity} onChange={(e) => setNewProduct({ ...newProduct, quantity: e.target.value })} />
                <button>Add Product</button>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default InventoryManagementPage;
