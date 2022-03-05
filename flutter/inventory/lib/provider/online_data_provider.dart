import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:inventory/abstractions.dart';
import 'package:inventory/models/inventory.dart';
import 'package:inventory/models/sale.dart';

final inventoryProvider =
    Provider<InventoryProvider>((ref) => InventoryProvider());

final saleProvider = Provider<SaleProvider>((ref) => SaleProvider());

class InventoryProvider with FirestoreCrudMixin {
  Stream<List<Inventory>> get allItems =>
      firestore.collection('Item').snapshots().map((snapshot) =>
          snapshot.docs.map((doc) => Inventory.fromJson(doc.data())).toList());
}

class SaleProvider with FirestoreCrudMixin {
  Stream<List<Sale>> get allItems =>
      firestore.collection('Sale').snapshots().map((snapshot) =>
          snapshot.docs.map((doc) => Sale.fromJson(doc.data())).toList());
}
