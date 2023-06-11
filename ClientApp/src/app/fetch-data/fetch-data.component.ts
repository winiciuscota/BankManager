import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public accounts: Account[] = [];
  public baseUrl = "https://localhost:7147/";
  public newAccount: newAccount = { name: '', initialBalance: 0 };
  public errorMessage: string = '';
  public action: string = 'new';
  public transactionAmount: number = 0;
  public selectedAccount: Account = { id: '', name: '', balance: 0, createdAt: new Date() };

  constructor(private httpClient: HttpClient) {
    this.loadAccounts();
  }

  public loadAccounts() {
    this.httpClient.get<Account[]>(this.baseUrl + 'accounts').subscribe(result => {
      this.accounts = result;
      this.errorMessage = '';
    }, error => this.setError(error.error));
  }

  public addAccount() {
    console.log(this.newAccount);
    this.httpClient.post(this.baseUrl + 'accounts', this.newAccount).subscribe(result => {
      this.loadAccounts();
    }, error => this.setError(error.error));
  }

  public delete(account : Account) {
    this.httpClient.delete(this.baseUrl + 'accounts/' + account.id).subscribe(result => {
      this.loadAccounts();
    }, error => this.setError(error.error));
  }

  public transact(account: Account, action: string) {
    this.httpClient.post(this.baseUrl + 'accounts/' + account.id + '/' + action, this.transactionAmount).subscribe(result => {
      this.loadAccounts();
    }, error => this.setError(error.error));
  }

  public setError(error: string) {
    console.error(error);
    this.errorMessage = error;
  }

  public setAction(action: string, account: Account = { id: '', name: '', balance: 0, createdAt: new Date() }) {
    this.action = action;
    this.selectedAccount = account;
  }


}



interface Account {
  id: string;
  name: string;
  balance: number;
  createdAt: Date;
}

interface newAccount {
  name: string;
  initialBalance: number;
}

