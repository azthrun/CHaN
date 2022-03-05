import 'package:cloud_firestore/cloud_firestore.dart';
import 'package:inventory/abstractions.dart';
import 'package:inventory/models/inventory.dart';

class Sale implements GenericModel {
  @override
  String? id;
  @override
  Timestamp? updatedAt;

  Inventory? inventory;
  int? quantity;
  double? price;
  Timestamp? saleDate;
  String? buyer;
  String? address;
  String? tracking;
  String? remarks;

  Sale({
    this.id,
    required this.inventory,
    required this.quantity,
    required this.price,
    required this.saleDate,
    this.buyer,
    this.address,
    this.tracking,
    this.remarks,
    required this.updatedAt,
  });

  factory Sale.fromJson(Map<String, dynamic> json) {
    return Sale(
      id: json['id'],
      inventory: Inventory.fromJson(json['inventory']),
      quantity: json['quantity'] as int?,
      price: json['price'] as double?,
      saleDate: json['saleDate'] as Timestamp?,
      buyer: json['buyer'],
      address: json['address'],
      tracking: json['tracking'],
      remarks: json['remarks'],
      updatedAt: json['updatedAt'] as Timestamp?,
    );
  }

  @override
  Map<String, dynamic> toJson({bool keepUpdatedAt = true}) {
    final map = {
      'id': id,
      'inventory': inventory!.toJson(),
      'quantity': quantity,
      'price': price,
      'saleDate': saleDate,
      'buyer': buyer,
      'address': address,
      'tracking': tracking,
      'remarks': remarks,
      'updatedAt': updatedAt,
    };
    map.removeWhere((key, value) => value == null);
    return map;
  }
}
