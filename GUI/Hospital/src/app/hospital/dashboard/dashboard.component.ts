import { Component,OnInit ,NgZone } from '@angular/core';
import JsBarcode from 'jsbarcode';

declare var bootstrap: any; 
@Component({
  selector: 'app-hospital-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class HospitalDashboardComponent implements OnInit {
  searchTerm: string = '';
  users: any[] = [];
  filteredUsers: any[] = [];
  //bind backend
  private baseUrl = 'https://localhost:44338/';
  constructor(private zone: NgZone) {}

  ngOnInit(): void {
    // Dummy users for demo
    this.users = [
      {
        id: 101,
        name: 'Amit Sharma',
        age: 32,
        bloodGroup: 'B+',
        city: 'Mumbai',
        barcode: 'USR101'
      },
      {
        id: 102,
        name: 'Ritika Das',
        age: 28,
        bloodGroup: 'A-',
        city: 'Delhi',
        barcode: 'USR102'
      }
    ];
    this.filteredUsers = this.users;
    setTimeout(() => this.generateBarcodes(), 0);
  }

  onSearch(): void {
    const term = this.searchTerm.toLowerCase();
    this.filteredUsers = this.users.filter(user =>
      user.name.toLowerCase().includes(term) ||
      user.bloodGroup.toLowerCase().includes(term) ||
      user.city.toLowerCase().includes(term)
    );
    setTimeout(() => this.generateBarcodes(), 0);
  }

  generateBarcodes() {
    this.filteredUsers.forEach(user => {
      const canvas = document.getElementById(`barcode-${user.id}`) as HTMLCanvasElement;
      if (canvas) {
        JsBarcode(canvas, user.barcode, {
          format: 'CODE128',
          displayValue: true,
          fontSize: 14,
          height: 60,
        });
      }
    });
  }

   // Example method after barcode scan
  onBarcodeScanned(barcode: string) {
    console.log('Scanned Barcode:', barcode);

    // You can send email here via API...
    // After that, show success modal
    this.showSuccessModal();
  }

  showSuccessModal() {
  this.zone.run(() => {
    setTimeout(() => {
      const modalElement = document.getElementById('successModal');
      if (modalElement) {
        const modal = new (window as any).bootstrap.Modal(modalElement);
        modal.show();
      }
    }, 100);
  });
}
}
